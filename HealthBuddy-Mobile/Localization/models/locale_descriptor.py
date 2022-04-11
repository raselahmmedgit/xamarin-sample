#!/usr/bin/env python
# -*- coding: utf-8 -*-

class LocaleDescriptor:
    def __init__(self, localeName: str):
        self.localeName = localeName
        self.items = {}