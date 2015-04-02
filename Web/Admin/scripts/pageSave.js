/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: pageSave.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2008-2009
 * 
 *  Description:
 * 
 *  Notes:
 *      $Id: pageSave.js 77905 2008-04-22 13:49:28Z ACHIEVO\levin.feng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

//Get global setting object information.
function PageGlobalItems()
{
    this.arrGlobal = new Array();
    this.AddPageItem = AddPageItem;
    this.UpdatePageItem = UpdatePageItem;
    this.GetPageChangedItemsByPageId = GetPageChangedItemsByPageId;
    this.RemovePageChangedItemsByPageId = RemovePageChangedItemsByPageId;
}

//Get module setting object information.
function PageModuleItems()
{
    this.arrMoudle = new Array();
    this.AddPageItem = AddPageItem;
    this.UpdatePageItem = UpdatePageItem;
    this.GetPageChangedItemsByPageId = GetPageChangedItemsByPageId;
    this.RemovePageChangedItemsByPageId = RemovePageChangedItemsByPageId;
}

//Get regristration setting object information.
function PageRegistrationItems()
{
    this.arrRegistration = new Array();
    this.AddPageItem = AddPageItem;
    this.UpdatePageItem = UpdatePageItem;
    this.GetPageChangedItemsByPageId = GetPageChangedItemsByPageId;
    this.RemovePageChangedItemsByPageId = RemovePageChangedItemsByPageId;
}

//Get inspection setting object information.
function PageInspectionItems()
{
    this.arrInspection = new Array();
    this.AddPageItem = AddPageItem;
    this.UpdatePageItem = UpdatePageItem;
    this.GetPageChangedItemsByPageId = GetPageChangedItemsByPageId;
    this.RemovePageChangedItemsByPageId = RemovePageChangedItemsByPageId;
}

//Get application status setting object information.
function PageAppStatusItems()
{
    this.arrAppStatus = new Array();
    this.AddPageItem = AddPageItem;
    this.UpdatePageItem = UpdatePageItem;
    this.GetPageChangedItemsByPageId = GetPageChangedItemsByPageId;
    this.RemovePageChangedItemsByPageId = RemovePageChangedItemsByPageId;
}

//Update object element by type.
function UpdatePageItem(type,item)
{
    var arr;
    var index = -1;
    var isExist = false;
    switch(type)
    {
        case 'global':
            arr = this.arrGlobal;
            break;
        case 'module':
            arr = this.arrMoudle;
            break;
        case 'registration':
            arr = this.arrRegistration;
            break;
        case 'inspection':
            arr = this.arrInspection;
            break;
        case 'appstatus':
            arr = this.arrAppStatus;
            break;
        default:
            arr = null;
            break;
    }
    
    if(arr != null)
    {
        if(type == 'global' || type == 'registration' || type == 'inspection')
        {
            for(var i = 0; i< arr.length; i++)
            {
                if(arr[i].PageId == item.PageId && arr[i].ModuleName == item.ModuleName)
                {
                    isExist = true;
                    index = i;
                    break;
                }
            }
            
            if(isExist)
            {
                arr[index] = item;
            }
            else
            {
                this.AddPageItem(type,item);
            }
        }
        else if(type == 'module' || type == 'appstatus')
        {
            for(var i = 0; i< arr.length; i++)
            {
                if(arr[i].PageId == item.PageId && arr[i].ModuleName == item.ModuleName 
                        && arr[i].NodeId == item.NodeId && (type == 'appstatus' ||  arr[i].LabelKey == item.LabelKey))
                {
                    isExist = true;
                    index = i;
                    break;
                }
            }
            
            if(isExist)
            {
                arr[index] = item;
            }
            else
            {
                this.AddPageItem(type,item);
            }
        }

    }
}

//Add object element by type.
function AddPageItem(type,item)
{
    var arr;
    switch(type)
    {
        case 'global':
            arr = this.arrGlobal;
            break;
        case 'module':
            arr = this.arrMoudle;
            break;
        case 'registration':
            arr = this.arrRegistration;
            break;
        case 'inspection':
            arr = this.arrInspection;
            break;
        case 'appstatus':
            arr = this.arrAppStatus;
            break;
        default:
            arr = null;
            break;
    }
    
    if(arr != null)
    {
        var length = arr.length;
        arr[length] = item;
    }
}

//Packge ojbect element to array.
function GetPageChangedItemsByPageId(pageId)
{
    var moduleName = Ext.Const.ModuleName; 
    var resultItems;
    if(pageId == 'global')
    {
        resultItems = new PageGlobalItems();
        
        for(var i = 0;i < this.arrGlobal.length;i++)
        {
            if(this.arrGlobal[i].PageId == pageId && this.arrGlobal[i].ModuleName == moduleName)
            {
                resultItems.AddPageItem(pageId,this.arrGlobal[i]);
            }
        }
    }
    else if(pageId == 'module')
    {
        resultItems = new PageModuleItems();
        
        for(var i = 0;i < this.arrMoudle.length;i++)
        {
            if(this.arrMoudle[i].PageId == pageId && this.arrMoudle[i].ModuleName == moduleName)
            {
                resultItems.AddPageItem(pageId,this.arrMoudle[i]);
            }
        }
    }
    else if(pageId == 'registration')
    {
        resultItems = new PageRegistrationItems();
        
        for(var i = 0;i < this.arrRegistration.length;i++)
        {
            if(this.arrRegistration[i].PageId == pageId && this.arrRegistration[i].ModuleName == moduleName)
            {
                resultItems.AddPageItem(pageId,this.arrRegistration[i]);
            }
        }
    }
    else if(pageId == 'inspection')
    {
        resultItems = new PageInspectionItems();
        
        for(var i = 0;i < this.arrInspection.length;i++)
        {
            if(this.arrInspection[i].PageId == pageId && this.arrInspection[i].ModuleName == moduleName)
            {
                resultItems.AddPageItem(pageId,this.arrInspection[i]);
            }
        }
    }
    else if(pageId == 'appstatus')
    {
        resultItems = new PageAppStatusItems();
        
        for(var i = 0;i < this.arrAppStatus.length;i++)
        {
            if(this.arrAppStatus[i].PageId == pageId && this.arrAppStatus[i].ModuleName == moduleName)
            {
                resultItems.AddPageItem(pageId,this.arrAppStatus[i]);
            }
        }
    }

    return resultItems;
}

//Remove ojbect element to array.
function RemovePageChangedItemsByPageId(pageId)
{
    var moduleName = Ext.Const.ModuleName; 
    var resultItems;
    if(pageId == 'global')
    {
        resultItems = new PageGlobalItems();
        
        for(var i = 0;i < this.arrGlobal.length;i++)
        {
            if(this.arrGlobal[i].PageId != pageId && this.arrGlobal[i].ModuleName != moduleName)
            {
                resultItems.AddPageItem(pageId,this.arrGlobal[i]);
            }
        }
        pageGlobalItems = resultItems;
    }
    else if(pageId == 'module')
    {
        resultItems = new PageModuleItems();
        
        for(var i = 0;i < this.arrMoudle.length;i++)
        {
            if(this.arrMoudle[i].PageId != pageId && this.arrMoudle[i].ModuleName != moduleName)
            {
                resultItems.AddPageItem(pageId,this.arrMoudle[i]);
            }
        }
        pageModuleItems = resultItems;
    }
    else if(pageId == 'registration')
    {
        resultItems = new PageRegistrationItems();
        
        for(var i = 0;i < this.arrRegistration.length;i++)
        {
            if(this.arrRegistration[i].PageId != pageId && this.arrRegistration[i].ModuleName != moduleName)
            {
                resultItems.AddPageItem(pageId,this.arrRegistration[i]);
            }
        }
        pageRegistrationItems = resultItems;
    }
    else if(pageId == 'inspection')
    {
        resultItems = new PageInspectionItems();
        
        for(var i = 0;i < this.arrInspection.length;i++)
        {
            if(this.arrInspection[i].PageId != pageId && this.arrInspection[i].ModuleName != moduleName)
            {
                resultItems.AddPageItem(pageId,this.arrInspection[i]);
            }
        }
        pageInspectionItems = resultItems;
    }
    else if(pageId == 'appstatus')
    {
        resultItems = new PageAppStatusItems();
        
        for(var i = 0;i < this.arrAppStatus.length;i++)
        {
            if(this.arrAppStatus[i].PageId != pageId && this.arrAppStatus[i].ModuleName != moduleName)
            {
                resultItems.AddPageItem(pageId,this.arrAppStatus[i]);
            }
        }
        pageAppStatusItems = resultItems;
    }
}
