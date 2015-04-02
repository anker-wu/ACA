/**
* <pre>
* 
*  Accela Citizen Access
*  File: ExtConst.js
* 
*  Accela, Inc.
*  Copyright (C): 2009-2014
* 
*  Description:
* 
*  Notes:
* $Id: ExtConst.js 72643 2009-04-06 09:52:06Z $.
*  Revision History
*  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
*  04/05/2009     		Vera.Zhao  				Initial.  
* </pre>
*/


/*
||Define ext const
*/
Ext.Const = function() {
    return {
        init: function() { }
    };
} ();

Ext.BLANK_IMAGE_URL = 'resources/images/default/s.gif';
/*
||Feature:The tab title
*/
Ext.Const.BasicTab = 'Basic Configuration';
Ext.Const.RegTab = 'Registration Configuration';
Ext.Const.HomeTab = 'Home Tab Configuration';
Ext.Const.ModuleTab = 'Module Configuration';

/*
||Feature:The tree JSON data
*/
Ext.Const.HomeTree = 'TreeJSON/HomeTab.aspx';

/*
||Feature:The properties panel title
*/
Ext.Const.FieldProp = 'Field Properties';
Ext.Const.TemplateFieldProp = 'Template Field Properties';
Ext.Const.SectionProp = 'Section Properties';
Ext.Const.PageProp = 'Page Properties';
Ext.Const.PermProp = 'Permissions';
Ext.Const.TabProp = 'Tab Order Properties';

Ext.Const.OpenedId = -1;
Ext.Const.DisplayModuleName = '';
Ext.Const.ModuleName = 'MODULE';
Ext.Const.CurrId = '';
Ext.Const.DefaultLang = '';
Ext.Const.IsSupportMultiLang = false;
Ext.Const.SplitChar = '\f';
Ext.Const.People = "People";
Ext.Const.Attachment = "ATTACHMENT";
Ext.Const.TemplateGenusType = "ListTemplate";

Ext.Const.SectionDict = new Array();
Ext.Const.SectionDict.push({ sectionId: '60001', permissionLevel: 'APO', permissionValue: 'ADDRESS' });
Ext.Const.SectionDict.push({ sectionId: '60002', permissionLevel: 'APO', permissionValue: 'PARCEL' });
Ext.Const.SectionDict.push({ sectionId: '60003', permissionLevel: 'APO', permissionValue: 'OWNER' });
Ext.Const.SectionDict.push({ sectionId: '60004', permissionLevel: 'People', permissionValue: 'License' });
Ext.Const.SectionDict.push({ sectionId: '60005', permissionLevel: 'People', permissionValue: 'Contact' });
Ext.Const.SectionDict.push({ sectionId: '60006', permissionLevel: 'CAP DESCRIPTION', permissionValue: '' });
Ext.Const.SectionDict.push({ sectionId: '60007', permissionLevel: 'CAP_GENERALSEARCH', permissionValue: '' });
Ext.Const.SectionDict.push({ sectionId: '60015', permissionLevel: 'DETAIL INFORMATION', permissionValue: '' });
Ext.Const.SectionDict.push({ sectionId: '60016', permissionLevel: Ext.Const.Attachment, permissionValue: '' });
Ext.Const.SectionDict.push({ sectionId: '60018', permissionLevel: 'EDUCATION', permissionValue: '' });
Ext.Const.SectionDict.push({ sectionId: '60084', permissionLevel: 'CONTINUING EDUCATION', permissionValue: '' });
Ext.Const.SectionDict.push({ sectionId: '60019', permissionLevel: 'EXAMINATION', permissionValue: '' });
Ext.Const.SectionDict.push({ sectionId: '60075', permissionLevel: 'SEARCH_FOR_PROVIDER', permissionValue: '' });
Ext.Const.SectionDict.push({ sectionId: '60076', permissionLevel: 'SEARCH_FOR_EDUCATIONANDEXAM', permissionValue: '' });
Ext.Const.SectionDict.push({ sectionId: '60086', permissionLevel: 'SEARCH_FOR_LICENSEE', permissionValue: '' });
Ext.Const.SectionDict.push({ sectionId: '60115', permissionLevel: 'SEARCH_FOR_FOOD_FACILITY', permissionValue: '' });
Ext.Const.SectionDict.push({ sectionId: '60119', permissionLevel: 'SEARCH_FOR_CERTIFIED_BUSINESS', permissionValue: '' });
Ext.Const.SectionDict.push({ sectionId: '60010', permissionLevel: 'APO', permissionValue: 'ADDRESS' });
Ext.Const.SectionDict.push({ sectionId: '60011', permissionLevel: 'APO', permissionValue: 'ADDRESS' });
Ext.Const.SectionDict.push({ sectionId: '60012', permissionLevel: 'APO', permissionValue: 'PARCEL' });
Ext.Const.SectionDict.push({ sectionId: '60013', permissionLevel: 'APO', permissionValue: 'OWNER' });
Ext.Const.SectionDict.push({ sectionId: '60127', permissionLevel: 'People', permissionValue: '' });
Ext.Const.SectionDict.push({ sectionId: '60129', permissionLevel: 'USERREGISTRATION', permissionValue: '' });
Ext.Const.SectionDict.push({ sectionId: '60130', permissionLevel: 'USERACCOUNT', permissionValue: '' });
Ext.Const.SectionDict.push({ sectionId: '60135', permissionLevel: 'ContactAddress', permissionValue: '' });
Ext.Const.SectionDict.push({ sectionId: '60137', permissionLevel: Ext.Const.Attachment, permissionValue: '' });
Ext.Const.SectionDict.push({ sectionId: '60140', permissionLevel: 'People', permissionValue: '' });
Ext.Const.SectionDict.push({ sectionId: '60141', permissionLevel: 'People', permissionValue: '' });
Ext.Const.SectionDict.push({ sectionId: '60142', permissionLevel: 'People', permissionValue: '' });
Ext.Const.SectionDict.push({ sectionId: '60143', permissionLevel: 'ContactAddress', permissionValue: '' });
Ext.Const.SectionDict.push({ sectionId: '60147', permissionLevel: 'USERREGISTRATION', permissionValue: '' });
Ext.Const.SectionDict.push({ sectionId: '60152', permissionLevel: 'USERACCOUNT', permissionValue: '' });
Ext.Const.SectionDict.push({ sectionId: '60148', permissionLevel: 'People', permissionValue: '' });
Ext.Const.SectionDict.push({ sectionId: '60149', permissionLevel: 'ContactAddress', permissionValue: '' });
Ext.Const.SectionDict.push({ sectionId: '60153', permissionLevel: 'People', permissionValue: '' });
Ext.Const.SectionDict.push({ sectionId: '60154', permissionLevel: 'People', permissionValue: '' });
Ext.Const.SectionDict.push({ sectionId: '60155', permissionLevel: 'ContactAddress', permissionValue: '' });
Ext.Const.SectionDict.push({ sectionId: '60159', permissionLevel: 'People', permissionValue: '' });
Ext.Const.SectionDict.push({ sectionId: '60160', permissionLevel: 'ContactAddress', permissionValue: '' });
Ext.Const.SectionDict.push({ sectionId: '60017', permissionLevel: 'ASSETS', permissionValue: '' });
Ext.Const.SectionDict.push({ sectionId: '60174', permissionLevel: 'EDUCATION', permissionValue: '' });
Ext.Const.SectionDict.push({ sectionId: '60175', permissionLevel: 'EXAMINATION', permissionValue: '' });
Ext.Const.SectionDict.push({ sectionId: '60176', permissionLevel: 'CONTINUING EDUCATION', permissionValue: '' });
Ext.Const.SectionDict.push({ sectionId: '60180', permissionLevel: 'People', permissionValue: '' });
Ext.Const.SectionDict.push({ sectionId: '60181', permissionLevel: 'People', permissionValue: '' });
Ext.Const.SectionDict.push({ sectionId: '60020', permissionLevel: 'ContactAddress', permissionValue: '' });
Ext.onReady(Ext.Const.init, Ext.Const);