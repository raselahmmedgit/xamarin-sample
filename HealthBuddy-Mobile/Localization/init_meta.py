#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""Script for working with multiple .Net Resource (.resx) files."""

import sys
import glob
import json
import os.path
import models.locale_descriptor as model
import const
import argparse

args_parser = argparse.ArgumentParser(description="Script scans for .Net Resource (.resx) files and stores them in metadata file.",
epilog="Example: init_meta.py ../src/ ./results/")
args_parser.add_argument("src_path", help="path to the folder with an app source code. Should correlate with paths in metadata file.")
args_parser.add_argument("results_path", help="path to a results folder.")

def main(args):
    dir_path = args.src_path
    walk_dir = os.path.abspath(dir_path)
    path = os.path.join(walk_dir,const.TargetPattern)
    results_path = args.results_path

    print("Working dir: " + path)

    # get all files with resx format
    files = glob.iglob(path, recursive = True)

    data = {}

    print("Scanning for resx files.") 

    for f in files:
        # split file names to detect language
        file_name = os.path.basename(f)
        file_parts = file_name.split(".")
        locale = const.DefaultLocaleName
        # If file consists of 3 parts. i.e. Localization.en.resx - then need to put into separate group for 'en' lacel
        # otherwise it is 'default' language which is omitted
        print("Processing: " + f)
        if(len(file_parts) == 3):
            # extract locale
            locale = file_parts[1]

        locale_descr = __getOrCreateLocaleDescriptor(data, locale)
        
        # get the feature name bsed on the module localization path structure {Feature_Name}/Resources/Localization.resx
        feature_name = __getFeatureNameFromPath(f)
        file_relative_path = __getFeatureRelativePath(f, walk_dir)
        locale_descr.items[feature_name] = file_relative_path

        #write results to corresponsing files
    for locale_descriptor in data.values():
        results_file_path = os.path.join(results_path, locale_descriptor.localeName  + ".json")
        __WriteLocaleDescriptorToFile(locale_descriptor, results_file_path)

    print("Operation completed.") 

def __getOrCreateLocaleDescriptor(dictionary, key) -> model.LocaleDescriptor:
    if key not in dictionary:
        dt = model.LocaleDescriptor(key)
        dictionary[key] = dt
        return dt
    else:
        return dictionary[key]

def __getFeatureNameFromPath(path:str) -> str:
        """ Get the feature name bsed on the module localization path structure {Feature_Name}/Resources/Localization.resx """
        dir_parts = os.path.dirname(path).split(os.path.sep)
        feature_name = dir_parts[-2]
        return feature_name

def __getFeatureRelativePath(fullPath:str, basePath: str) -> str:
        """ Get the feature relative path """        
        return str(os.path.relpath(fullPath, basePath))

def __CreateFolder(folder_path:str) -> str:
        """ Creates a folder """
        #create a fodler for results 
        os.makedirs(folder_path, exist_ok = True )

def __WriteLocaleDescriptorToFile(locale_descriptor: model.LocaleDescriptor, results_folder_path: str):
        """ Writes locale descriptor to a json file """
        #create a folder for results 
        folder_path = os.path.dirname(results_folder_path)
        __CreateFolder(folder_path)
        with open(results_folder_path, 'w', encoding='utf-8') as description_file:
            json.dump(locale_descriptor.__dict__, description_file, indent=4)

if __name__ == '__main__':
    args = args_parser.parse_args()
    main(args)