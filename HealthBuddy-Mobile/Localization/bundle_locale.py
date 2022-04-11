#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""Script for merging .Net Resource (.resx) files from provided metadata file into a single one."""

import sys
import glob
import json
import os.path
import shutil
import const
from resx import resx_manager
import argparse

args_parser = argparse.ArgumentParser(description="Script for merging .Net Resource (.resx) files from provided metadata file into a single one.",
epilog="Example: bundle_locale.py ../src/ ./results/default.json ./results/")
args_parser.add_argument("src_path", help="path to the folder with an app source code. Should correlate with paths in metadata file.")
args_parser.add_argument("metadata_file_path", help="path to a file containing metadata about localization resources.")
args_parser.add_argument("results_path", help="path to a results folder.")

def main(args):
    dir_path = args.src_path
    meta_file_path = args.metadata_file_path
    results_path = args.results_path
    
    print("Meta file: " + meta_file_path)
    print("Working dir: " + os.path.abspath(dir_path))
    
    with open(meta_file_path, 'r') as meta_file:
        meta_json = json.load(meta_file)
    print("Meta file loaded.")

    items = meta_json[const.MetaFileItemsFieldName]

    if len(items) == 0:
        print("No files to process. Completing.")
        return
        
    final_file_manager = resx_manager.ResxManager()
    final_file_manager.create()  

    # Merge resx files from metafile
    for entry in items:
        file = items[entry]
        print("Processing resource file: " + file)
        orig_file_path = os.path.join(dir_path,file)

        original_file_manager = resx_manager.ResxManager()
        original_file_manager.load(orig_file_path)
        for item in original_file_manager.getItems():
            key = entry+const.MergedFileEntrySeparator+item.key
            final_file_manager.add(key, item.value)
           
    first_item = list(items.values())[0]

    target_file_name = os.path.basename(first_item)
    target_file_path = os.path.join(results_path, target_file_name)
    final_file_manager.save(target_file_path)

    print("Operation completed.")

if __name__ == '__main__':
    args = args_parser.parse_args()
    main(args)