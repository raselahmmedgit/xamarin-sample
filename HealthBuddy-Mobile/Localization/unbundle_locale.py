#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""Script to split merged .Net Resource (.resx) file into feature based resource files according to metadata file."""

import sys
import glob
import json
import os.path
import shutil
import const
from resx import resx_manager
import argparse

args_parser = argparse.ArgumentParser(description="Script to split merged .Net Resource (.resx) file into feature based resource files according to metadata file.",
epilog="Example: unbundle_locale.py ../src/  results/Localization.resx results/default.json")
args_parser.add_argument("src_path", help="path to the folder with an app source code. Should correlate with paths in metadata file.")
args_parser.add_argument("resource_file_path", help="path to a combined resource file.")
args_parser.add_argument("metadata_file_path", help="path to a file containing metadata about localization resources.")

def main(args):
    dir_path = args.src_path
    resource_file_path = args.resource_file_path
    meta_file_path = args.metadata_file_path
    
    print("Meta file: " + meta_file_path)
    print("Working dir: " + os.path.abspath(dir_path))
    
    with open(meta_file_path, 'r') as meta_file:
        meta_json = json.load(meta_file)

    meta_items = meta_json[const.MetaFileItemsFieldName]
    print("Meta file loaded.")

    if len(meta_items) == 0:
        print("No files to process. Completing.")
        return
    
    resx_file_manager = resx_manager.ResxManager()
    resx_file_manager.load(resource_file_path)
    print("Resx file loaded.")    

    features_resx_managers = {}
    missing_features = []

    for localized_entry in resx_file_manager.getItems():
        merged_key = resx_manager.SplitMergedEntryName(localized_entry.key)

        # If the feature is not defined in metadata file - should log and skip
        if merged_key.feature not in meta_items:
            if merged_key.feature in missing_features:
                continue
            missing_features.append(merged_key.feature)
            print("Warning: Feautre '" + merged_key.feature + "' is not defined in the metadata file. skipping.")
            continue
        feature_path = os.path.join(dir_path,meta_items[merged_key.feature])
        manager = __getOrCreateManager(features_resx_managers, merged_key.feature, feature_path)
        manager.add(merged_key.key, localized_entry.value)

    for feature_resx_manager in features_resx_managers.values():
        feature_resx_manager.save_file_changes()
           
    print("Operation completed.")

def __getOrCreateManager(dictionary, key: str, path: str) -> resx_manager.ResxManager:
    if key not in dictionary:
        manager = resx_manager.ResxManager()
        manager.create_for_file(path)
        dictionary[key] = manager
        print("Processing resource file: " + path)
        return manager
    else:
        return dictionary[key]

if __name__ == '__main__':
    args = args_parser.parse_args()
    main(args)