#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""Script for adding .Net Resource (.resx) files for a new locale by copying resx files from provided metadata file."""

#from lxml import etree

import sys
import glob
import json
import os.path
import shutil
import const
import argparse

args_parser = argparse.ArgumentParser(description="Script for adding .Net Resource (.resx) files for a new locale by copying resx files from provided metadata file.",
epilog="Example: add_locale.py ../ ./default.json it")
args_parser.add_argument("src_path", help="path to the folder with an app source code. Should correlate with paths in metadata file.")
args_parser.add_argument("metadata_file_path", help="path to a file containing metadata about localization resources.")
args_parser.add_argument("locale", help="ISO format of locale to add.")

def main(args):
    dir_path = args.src_path
    meta_file_path = args.metadata_file_path
    locale = args.locale
    
    print("Meta file: " + meta_file_path)
    print("Working dir: " + os.path.abspath(dir_path))
    
    with open(meta_file_path, 'r') as meta_file:
        meta_json = json.load(meta_file)
    print("Meta file loaded.")

    items = meta_json[const.MetaFileItemsFieldName]
    
    if len(items) == 0:
        print("No files to process. Completing.")
        return

    for file in items.values():
        orig_file_path = os.path.join(dir_path,file)
        orig_file_dir_path = os.path.dirname(orig_file_path)  
                     
        # Check which locale file name format has been provided: default one "Localization.resx" or localized "Localization.it.resx"
        orig_file_name_parts = os.path.basename(file).split(".")
        if(len(orig_file_name_parts) == 3):
            # extract locale
            target_file_name = orig_file_name_parts[0] +"." + locale + "." +orig_file_name_parts[2]
        else:
            target_file_name = orig_file_name_parts[0] +"." + locale + "." +orig_file_name_parts[1]

        target_file_dir = orig_file_dir_path
        target_file_path = os.path.join(target_file_dir, target_file_name)
        print("Processing resource file: " + target_file_path)

        shutil.copy(orig_file_path, target_file_path) 

    print("Operation completed.")

if __name__ == '__main__':
    args = args_parser.parse_args()
    main(args)