/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: CombineButtonUtil.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: menu.js 72643 2009-07-28 09:52:06Z $.
 *  Revision History
 *  &lt;Date&gt;,       &lt;Who&gt;        &lt;What&gt;
 *  07/28/2009          Vera.Zhao          Initial.  
 * </pre>
 */

/*  Methods：
    item(key): Get item by key 
    add(key,value): Add an item
    batchAdd(keys,values): Add batched keys and values. If success, return true, otherwise,false.
    clear(): Clear all items
    containsKey(key): Check if the Dictionary contains the specific key
    remove(key): Delete the specific item by key
    tryGetValue(key): Get item value by key. If the key is not exist, return null.
*/

function Dictionary() {
    var me = this;    
    this.count = 0;
    this.keys = new Array(); 
    this.values = new Array(); 

    this.item = function(key)
    {
        var index = getElementIndex(key);
        if (index != -1) {
            return me.values[index];
        }
    }
    
    this.containsKey = function(key)
    {
        return getElementIndex(key) != -1;
    }

    this.add = function(key, value)	 
    {
        if (checkKey(key)) {
            me.keys[me.count] = key;
            me.values[me.count] = value;
            me.count++;
        }
    }
    
    this.batchAdd = function(keys,values)
    {
        var successed = false;

        if(keys != null && keys != undefined && values != null && values !=undefined )
        {
            if(keys.length == values.length && keys.length>0)    //the length for keys and values must be same
            {
                var allKeys = me.keys.concat(keys); 

                if(!isArrayElementRepeat(allKeys))   //check if has repeated keys
                {  
                    // add keys and values to Dictionary
                    me.keys = allKeys;
                    me.values = me.values.concat(values);
                    me.count = me.keys.length;
                    successed=true;
                }
            }
        }

        return successed;
    }

    this.clear = function()
    {
        if (me.count != 0) {
            me.keys.splice(0, me.count);
            me.values.splice(0, me.count);
            me.count = 0;
        }
    }
    
    this.remove = function(key)
    {
        var index = getElementIndex(key);
        
        if (index != -1) {
            me.keys.splice(index, 1);
            me.values.splice(index, 1);
            me.count--;
            return true;
        }
        else {
            return false;
        }
    }

    this.tryGetValue = function(key)
    {
        var index = getElementIndex(key);
        
        if (index != -1) {
            return me.values[index];
        }
        else {
            return null;
        }         
    }

    function checkKey(key)
    {
        if (key == null || key == undefined || key == "" || key == NaN){
            return false;
        }
        
        // if has same key in the Dictionary, return false.
        return getElementIndex(key) == -1;  
    }

    function getElementIndex(key)
    {
        var index = -1; 
        
        for( var i = 0; i < me.keys.length; i++){
           if (me.keys[i] == key) {
               index = i;
               break;
            }
        }
        
        return index;
    }
    
    function isArrayElementRepeat(array)
    {
        var repeated = false;

        if(array != null && typeof(array) != "undefined")
        {
            for(var i = 0; i < array.length-1; i++)
            {
                if (array[i] == array[i+1]) {
                    repeated = true;
                    break;
                }
            }
        }

        return repeated;
   }
}

Array.prototype.clear=function(){
  this.length=0;
}

Array.prototype.insertAt=function(index,obj){
  this.splice(index,0,obj);
}

Array.prototype.removeAt=function(index){
  this.splice(index,1);
}

function ButtonSettingModel() {
   this.buttonName;
   this.availableCapTypes = new Array();
   this.selectedCapTypes = new Array();
   this.appStatuses = new Array();
   
   // add added caps type to selected cap type list and 
   // remove them from available cap type list
   this.addCapTypes = function(capTypeNodes) {
       this.addCapTypeItems(this.selectedCapTypes, capTypeNodes);
       this.removeCapTypeItems(this.availableCapTypes, capTypeNodes);
   }
   
   // add removed caps type to available cap type list and 
   // remove them from selected cap type list
   this.removeCapTypes = function (capTypeNodes) {
       this.addCapTypeItems(this.availableCapTypes, capTypeNodes);
       this.removeCapTypeItems(this.selectedCapTypes, capTypeNodes);
   }
   
   this.addCapTypeItems = function (container, items) {
       if (items == null || items == undefined) {
           return;
       }
       
       for(var i=0; i<items.length; i++)
       {
         container.push(items[i].attributes);
       }
   }
   
   this.removeCapTypeItems = function (container, items) {
       if (container == null || items == null) {
          return;
       }
       
       for(var i=0;i<items.length;i++)
       {
          for(var j=0;j<container.length;j++)
          {
             // key: cap type
             var key = items[i].key ? items[i].key : items[i].attributes.key;
             
             if (key == container[j].key) {
                 container.removeAt(j);
                 break;
             }
          }
       }
   }
   
   this.addAppStatuses = function (appStatuses) {
      if (appStatuses != null && appStatuses != undefined) {
         for(var i=0;i<appStatuses.length;i++)
         {
            this.appStatuses.push(appStatuses[i]);
         }
      }
   } 
   
   this.removeAppStatusByCapTypes = function (capTypeNodes) {
       for(var i=0;i<capTypeNodes.length;i++)
       {
          for(var j=0;j<this.appStatuses.length;j++)
          {
             var appStatus = this.appStatuses[j];
             var capTypeKey = this.getCapTypeKeyByStatus(appStatus);
             
             if (JsonDecode(capTypeNodes[i].attributes.key) == capTypeKey) {
                 this.appStatuses.removeAt(j);
                 j--;
             }
          }
       }
   }
   
   this.hasContainCapType = function (capTypeKey) {
       var hasCapType = false;
       
       for(var i=0; i<this.selectedCapTypes.length; i++)
       {
          if (capTypeKey == this.selectedCapTypes[i].key) {
             hasCapType = true;
             break;
          }
       }
       
       return hasCapType;
   }
   
   this.getAppStatusByCapType = function (capTypeValue) {
      var appStatusArray = new Array();
      
      for(var i=0;i<this.appStatuses.length;i++)
      {
         var appStatus = this.appStatuses[i];
         var capTypeKey =  this.getCapTypeKeyByStatus(appStatus);
         
         if (JsonDecode(capTypeValue) == capTypeKey) {
             appStatusArray.push(appStatus);
         }
      }
       
       return appStatusArray;
   }

   this.getCapTypeKeyByStatus = function (appStatus) {
       return appStatus.group + '/' + appStatus.type + '/'
           + appStatus.subType + '/' + appStatus.category;
   }
}

