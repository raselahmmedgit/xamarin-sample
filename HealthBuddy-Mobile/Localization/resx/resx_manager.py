#!/usr/bin/env python
# -*- coding: utf-8 -*-

import xml.etree.ElementTree as ET
import xml.dom.minidom as minidom
import os
from typing import List
import const

RESOURCE_MARKUP = """<?xml version="1.0" encoding="utf-8"?>
<root>
    <xsd:schema id="root" xmlns="" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:msdata="urn:schemas-microsoft-com:xml-msdata">
    <xsd:import namespace="http://www.w3.org/XML/1998/namespace" />
    <xsd:element name="root" msdata:IsDataSet="true">
      <xsd:complexType>
        <xsd:choice maxOccurs="unbounded">
          <xsd:element name="metadata">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name="value" type="xsd:string" minOccurs="0" />
              </xsd:sequence>
              <xsd:attribute name="name" use="required" type="xsd:string" />
              <xsd:attribute name="type" type="xsd:string" />
              <xsd:attribute name="mimetype" type="xsd:string" />
              <xsd:attribute ref="xml:space" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name="assembly">
            <xsd:complexType>
              <xsd:attribute name="alias" type="xsd:string" />
              <xsd:attribute name="name" type="xsd:string" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name="data">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name="value" type="xsd:string" minOccurs="0" msdata:Ordinal="1" />
                <xsd:element name="comment" type="xsd:string" minOccurs="0" msdata:Ordinal="2" />
              </xsd:sequence>
              <xsd:attribute name="name" type="xsd:string" use="required" msdata:Ordinal="1" />
              <xsd:attribute name="type" type="xsd:string" msdata:Ordinal="3" />
              <xsd:attribute name="mimetype" type="xsd:string" msdata:Ordinal="4" />
              <xsd:attribute ref="xml:space" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name="resheader">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name="value" type="xsd:string" minOccurs="0" msdata:Ordinal="1" />
              </xsd:sequence>
              <xsd:attribute name="name" type="xsd:string" use="required" />
            </xsd:complexType>
          </xsd:element>
        </xsd:choice>
      </xsd:complexType>
    </xsd:element>
  </xsd:schema>
  <resheader name="resmimetype">
    <value>text/microsoft-resx</value>
  </resheader>
  <resheader name="version">
    <value>2.0</value>
  </resheader>
  <resheader name="reader">
    <value>System.Resources.ResXResourceReader, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
  <resheader name="writer">
    <value>System.Resources.ResXResourceWriter, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
</root>
"""

class ResourceItem:
    def __init__(self, key, value):
        self.key = key
        self.value = value

class MergedResourceKey:
    def __init__(self, feature, key):
      self.feature = feature
      self.key = key

def CreateMergedEntryName(feature : str, key: str ) -> str:
    return feature+const.MergedFileEntrySeparator+key

def SplitMergedEntryName(key: str ) -> MergedResourceKey:
    splitted = str.split(key, const.MergedFileEntrySeparator)
    return  MergedResourceKey(splitted[0], splitted[1])

class ResxManager:

    def __indent(self, elem, level=0) -> None:
        i = "\r\n" + level*"  "
        if len(elem):
            if not elem.text or not elem.text.strip():
                elem.text = i + "  "
            if not elem.tail or not elem.tail.strip():
                elem.tail = i
            for elem in elem:
                self.__indent(elem, level+1)
            if not elem.tail or not elem.tail.strip():
                elem.tail = i
        else:
            if level and (not elem.tail or not elem.tail.strip()):
                elem.tail = i

    def __remove_blanks(self, node) -> None:
        for x in node.childNodes:
            if x.nodeType == minidom.Node.TEXT_NODE:
                if x.nodeValue:
                    x.nodeValue = x.nodeValue.strip()
            elif x.nodeType == minidom.Node.ELEMENT_NODE:
                self.__remove_blanks(x)

    def __prettify(self, elem : ET.Element) -> str:
        """Return a pretty-printed XML string for the Element."""
        rough_string = ET.tostring(elem, encoding="utf-8")
        reparsed = minidom.parseString(rough_string)
        self.__remove_blanks(reparsed)
        pretty_xml = reparsed.toprettyxml(indent='  ', encoding="utf-8")
        return pretty_xml

    def create(self) -> None:
        markup = RESOURCE_MARKUP.replace('\n','').replace('  ','')
        self._xml = ET.fromstring(markup)

    def create_for_file(self, path : str) -> None:
        markup = RESOURCE_MARKUP.replace('\n','').replace('  ','')
        self._xml = ET.fromstring(markup)
        self._path = path

    def add(self, key: str, value: str) -> None:
        element = ET.SubElement(self._xml, "data", attrib={"name":key, "xml:space":"preserve"})
        value_element = ET.SubElement(element, "value").text = value        

    def save(self, path : str) -> None:
        # remove spaces, beautify format
        pretty_xml = self.__prettify(self._xml)
        folder_path = os.path.dirname(path)
        self.__CreateFolder(folder_path)
        with open(path, "wb") as writter:
            writter.write(pretty_xml)
    
    def save_file_changes(self) -> None:
        self.save(self._path)

    def load(self, path : str) -> None:
        xml = ET.parse(path).getroot()
        self._path = path
        self._xml = xml

    def getItems(self) -> List[ResourceItem]:
        items = []
        for xmlItem in self._xml.findall("data"):
            name = xmlItem.get("name")
            value = xmlItem.find("value").text
            item = ResourceItem(name, value)
            items.append(item)
        return items

    def __CreateFolder(self, folder_path:str) -> str:
        """ Creates a folder """
        #create a fodler for results 
        os.makedirs(folder_path, exist_ok = True )