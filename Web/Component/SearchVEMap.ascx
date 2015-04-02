<%@ Import namespace="Accela.ACA.Common"%>
<%@ Import namespace="Accela.ACA.Web.Common"%>
<%@ Control Language="C#" AutoEventWireup="true" Inherits="Accela.ACA.Web.Component.SearchVEMap" Codebehind="SearchVEMap.ascx.cs" %>

<% if (IsShowMap)
   {%> 
<script type="text/javascript" src='<%=ConfigManager.VEMapURI %>' ></script>


<script type="text/javascript"> 
     Array.prototype.remove = function(from, to) 
    {     
      this.splice(from, from < 0 ? this.length + (to || from || 1) : to || from || 1);
      return this.length;
    };
    //when page load,load this mapS   

      var mapS = null; 
      var findPlaceResultsS = null;
      var adArrayS;
      var argsControlIdS="<%=hiddenAddressS.ClientID %>";    
      var searchedS=false;
      // This flag is used to locate default country when no address is found
      var addressFoundS = false;
      
      function GetMapS() 
      {     
        
         callNumber = 0;
         if(mapS == null)
         {
            mapS = new VEMap('VEMapS');
            mapS.LoadMap();  
            document.getElementById("MSVE_navAction_container").className = "MSVE_Dashboard_V6 notraffic MSVE_OrthoView collapsed";
            mapS.Find(null, '<%=StandardChoiceUtil.GetDefaultCountry()%>');        
         }
      }
      
      function FindS()
      {         
          var chk=document.getElementById('chkShowS');
          ShowHideDivS(chk);              
          if(chk.checked && searchedS==false)
          {            
             HideControlS();
             getAddressArrayS();   
             ResizeMapS();                     
             if(adArrayS!=null && adArrayS !=undefined && adArrayS.length>0)
             {                
                 mapS.Clear();               
                 if(adArrayS.length==1 && adArrayS[0].length==0)//no address then show title div
                 {
                    var valueStr = document.getElementById(argsControlIdS).NoAddressNumber;                    
	                AddControlS(valueStr);	                
                 }
                 mapFindS();
                 if (addressFoundS == false) {
                    mapS.Find(null, '<%=StandardChoiceUtil.GetDefaultCountry()%>');
                 }
                 addressFoundS = false;
                 searchedS=true;
             }  
         }     
      }

     function GetCoordinatesS(layer, resultsArray, places, hasMore, veErrorMessage)
     {      
        if(places!=null && places.length>0)
        {               
            addressFoundS = true;
	        findPlaceResultsS = places[0].LatLong;
	        var myShape = new VEShape(VEShapeType.Pushpin, findPlaceResultsS);
	        var lineIndex = 0 ;
	        var permits = getPermitListS(adArrayS[0]);
	        myShape.SetTitle('<div id="sharpTitleS">');
	        for(var i=0;i<permits.length;i++)
	        {           
	            //[0]=PermitNumber [1]=PermitType [2]=CAPId [3]LineIndex       
	            var permit = permits[i].split('<%=ACAConstant.SPLIT_CHAR %>');
	            var capids=	permit[2].split('_');   
	            var oldTitle = myShape.GetTitle();  
	            var moduleName = permit[4];
	            lineIndex = permit[5]; 
	            if(oldTitle.length>0)
	            {
	                oldTitle=oldTitle+'<br/>';
	            } 
	                 
	            myShape.SetTitle(oldTitle+permit[0]+'<br/>'+permit[1]+'<br/><br/>'
	            +'<a style="FONT-WEIGHT: bold; FONT-SIZE: x-small; COLOR: #999999;text-decoration: none;">'
	            + adArrayS[0]
	            +'</a><br/><br/>'
	            +'<table role="presentation" style="height:50px;width:100%;border:0px;background-color:#dcdcdc;"><tr><td>'
	            +'<a style="color:cornflowerblue; font-weight: bold; font-size: small;" href="../Cap/CapDetail.aspx?Module='+ moduleName +'&TabName='+ moduleName + ''
	            +'&capID1='
	            +capids[0]
	            +'&capID2='+capids[1]
	            +'&capID3='+capids[2]
	            +'&<%= ACAConstant.CAP_AGENCY_CODE%>='+permit[3] +'">More Info...</a>'
	            +'</td></tr></table>');
	        }  
	        myShape.SetTitle(myShape.GetTitle()+'</div>');
	        
	        if(lineIndex > 0)
	        {
	        	myShape.SetCustomIcon(
                "<img onmouseover='SetInfoBoxStyleS()' src='../App_Themes/default/assets/pushpin" + lineIndex + ".gif'/>"); 
                
	            mapS.AddShape(myShape);   
	        }

	    }
	     
	     if(adArrayS != null && adArrayS.length>0)
	     {
             adArrayS.remove(0);
             mapFindS();
	     }
	     if(adArrayS.length==0)
	     {       
	        var valueStr = document.getElementById(argsControlIdS).NoAddressNumber;
	        AddControlS(valueStr);	         
	     }	   
     }
     
     function mapFindS()
     { 
        if(adArrayS != null){    
             if(adArrayS.length>0 && adArrayS[0].length==0)//item of adArrayS may be  null,remove the item
             {
                adArrayS.remove(0); 
             }
             
             if(adArrayS.length>0)//can continue to find
             {
                mapS.Find(null,adArrayS[0], null, null, null, null, true, true, true, true, GetCoordinatesS);
             }
         }
     } 
     
    function ShowHideDivS(chk)
     {
        var divIn=document.getElementById('VEMapS');
        var placeListPanel=document.getElementById("VEMapS_veplacelistpanel");
        if(chk.checked)
        { 
             divIn.style.display='block'; 
             mapS.ShowControl(placeListPanel);  
             showLineNumS(true);     
        }
        else
        {
            divIn.style.display='none';
            if(mapS != null)
            {
               mapS.HideControl(placeListPanel);
            }
            HideControlS();
            showLineNumS(false);
        }
     }
     
     function CloseMap()
     {
        var chk=document.getElementById('chkShowS');
        
        if(chk!=null)
        {
            chk.checked=false;
        }
        
        if(typeof(mapS) != 'undefined' && mapS != null)
        {
            mapS.ClearInfoBoxStyles();
            DeleteInfoBoxTitleS();
            DeleteControlS();
            mapS.Clear();
        }
        
        if(typeof(map) != 'undefined' && map != null)
        {
            map.ClearInfoBoxStyles();
            DeleteInfoBoxTitle();
        }
        
        ShowHideDivS(chk);
     } 
     
     function SetInfoBoxStyleS()
     {
        if(mapS != null)
        {
            mapS.SetInfoBoxStyles(); 
        }
     }
     
    function DeleteInfoBoxTitleS()
    {
        var title=document.getElementById("sharpTitleS");
        if (title != null)
        {
           title.parentNode.removeChild(title);
        }
    }
     
     function showLineNumS(isShow)
     {
        var permitList2= document.getElementById("ctl00_PlaceHolderMain_dgvPermitList_gdvPermitList");
          if(typeof(permitList2)!="undefined" && permitList2 != null)
          {
            var length=permitList2.rows.length;
        
            if(permitList2.attributes["PageCount"].value>1)
            {
                length=length-1;
            }
             
             for(var i=1;i<length;i++)
             {   
                 var lineNum;         
                 var tdLineNum = permitList2.rows[i].cells[0];
                
                if(isFireFox() || isSafari())     
                 {
                    lineNum = permitList2.rows[i].cells[0].childNodes[1];
                 }
                 else
                 {
                    lineNum = permitList2.rows[i].cells[0].childNodes[0];
                 }

                 if (typeof (lineNum.style) != 'undefined' && tdLineNum.firstChild.id != permitList2.id + "_ctl13_lb4btnExport")
                 {
                  if(isShow) 
                  {
                    if ( isFireFox() || isSafari())
                    {
                       lineNum.style.visibility="visible";
                       tdLineNum.style.visibility="visible";
                       lineNum.style.display="";
                       tdLineNum.style.display="";
                    }
                    else 
                    {
                       tdLineNum.style.display="inline";
                       lineNum.style.display="inline";
                    }   

                  }
                  else 
                  {
                       tdLineNum.style.display="none";
                       lineNum.style.display="none";
                  }
                }
            }
        }
     }
     
      function getAddressArrayS()
     {
         var valueStr = document.getElementById(argsControlIdS).value; 
         if(valueStr.length==0)
         {
             mapS.Clear();               
             var addrNum = document.getElementById(argsControlIdS).NoAddressNumber;                    
             AddControlS(addrNum);	                
            // mapFindS();
             if(adArrayS != null)
             {
               adArrayS.remove(0); 
             }
             return '';
         }
         
         var argsArray = valueStr.split('<%=ACAConstant.SPLIT_CHAR1 %>');
         adArrayS=new Array(argsArray.length);
         for(var i=0;i<argsArray.length;i++)
         {                     
             adArrayS[i]=argsArray[i].split('<%=ACAConstant.SPLIT_CHAR2 %>')[0];
             //alert(adArray[i]);
         }                                      
         return adArrayS;
     }
     
     function getPermitListS(ad)
     {
        var valueStr = document.getElementById(argsControlIdS).value; 
        if(valueStr.length==0)
        {
             return '';
        }
       var arrPermits =null;
       
       // split address + permits
       var argsArray = valueStr.split('<%=ACAConstant.SPLIT_CHAR1 %>');
       for(var i=0;i<argsArray.length;i++)
       {
           //[0] is address
           var item = argsArray[i].split('<%=ACAConstant.SPLIT_CHAR2 %>')[0]
           
           if(item==ad)
           {
              // [1] is permits
              var permits = argsArray[i].split('<%=ACAConstant.SPLIT_CHAR2 %>')[1];
              
              //alert(permits);
              arrPermits = permits.split("<%=ACAConstant.SPLIT_CHAR3 %>");  
              //alert(arrPermits[0]);                
           }
       }
      //alert(result);
       return arrPermits;
     }
    
    function AddControlS(noAddressNumber)
     {      
        DeleteControlS();  
        if(noAddressNumber == undefined || parseInt(noAddressNumber)==0)
        {
            return;
        }          
        var el = document.createElement("div"); 
        el.id = "divNoAddressS"; 
        el.style.top = 0;          
        el.style.left = '480px';  
        el.style.zIndex="0";      
        el.style.border = "2px solid black";
        el.style.background = "White";
        el.innerHTML = "<table role='presentation' border='0' cellpadding='0' cellspacing='0'><tr><td style='background-color:PowderBlue;font-size:x-small' align='right'><label onclick='javascript:HideControlS()'>X</label></td></tr><tr><td style='font-size:x-small'> "
        + noAddressNumber 
        + " permits did not have address details and cannot be &nbsp<br/>displayed!</td></tr></table>";      
        mapS.AddControl(el);
        addShimS(el);        
     }
     
     function DeleteControlS()
     {
        var myControl = document.getElementById("divNoAddressS");

        if (myControl!=null)
        {
           var myControlID = myControl.id;
           mapS.DeleteControl(myControl);          
        }
        else
        {          
           return;
        }
        
        var myShimS = document.getElementById("myShimS");

        if (myShimS!=null)
        {
           myShimS.parentNode.removeChild(myShimS);
        }
        myShimS = null;      
     }

     
     function addShimS(el)
     {       
        var shim = document.createElement("iframe");
        shim.id = "myShimS";
        shim.frameBorder = "0";
        shim.style.position = "absolute";
        shim.style.zIndex = "1";
        shim.style.top  = el.offsetTop;
        shim.style.left = el.offsetLeft;
        shim.width  = el.offsetWidth;
        shim.height = el.offsetHeight;
        el.shimElement = shim;
        el.parentNode.insertBefore(shim, el);
     }


    function HideControlS()
     {
        var myControl = document.getElementById("divNoAddressS");
        if (myControl!=null)
        {
           mapS.HideControl(myControl);           
        }
        else
        {          
           return;
        }
     }
     
     function ShowControlS()
     {
        var myControl = document.getElementById("divNoAddressS");
        if (myControl!=null)
        {
           mapS.ShowControl(myControl);       
        }
        else
        {          
           return;
        }
     }
     
     //auto fit search map position when ASI expand/collapse.
    function FitMapSPosition()
    {
        var mapS=document.getElementById("VEMapS");
        
        var noAddressS = document.getElementById("divNoAddressS");
        var shimS = document.getElementById("myShimS");
        
        var permitListS=document.getElementById("VEMapS_veplacelistpanel");
        
        //if map is null, it needn't adjust search map position.
        if(mapS == null || mapS =="undefined")
        {
            return;
        }
        //if not-adress list pad isn't null, need to fix position.
        if(noAddressS != null && noAddressS !="undefined")
        {
            noAddressS.style.top  = mapS.offsetTop+'px' ;
        }
        
        if(shimS != null && shimS != "undefined")
        {
            shimS.style.top = mapS.offsetTop+'px';
        }
        
        //if permit list pad isn't null, need to fix position.
        if(permitListS != null && permitListS !="undefined")
        {
            permitListS.style.top = parseInt(mapS.offsetTop+100)+'px' ;
        }
    }
     
    function AdjustSearchPromptDiv(aheight)
    {        
         var myControl = document.getElementById("divNoAddressS");
         if(myControl!=null)
         {
            var topValue=myControl.style.top.replace('px','');
            var newTopValue=parseInt(topValue)+aheight;
            myControl.style.top=newTopValue.toString()+'px';
         }
    }     
     
     
     function setPostBackInfoS(ads,noAddressNumber)
     {       
        var myControl=document.getElementById(argsControlIdS);
        myControl.value=ads;
        myControl.NoAddressNumber=noAddressNumber;     
     }    

    function ShowOnMapDiv(show)
    {      
        var myDiv = document.getElementById('divShowOnMap');
        var divIn=document.getElementById('VEMapS');
        if(myDiv!=null && divIn!=null)
        {
            if(show=="true")
            {                
                myDiv.style.display="block";
                var myCheck=document.getElementById('chkShowS');
                if(myCheck!=null)
                {
                    ShowHideDivS(myCheck);
                }
                
            }
            else
            {
                myDiv.style.display="none";                
                divIn.style.display="none";
                HideControlS();
            }
        }
                    
    } 
    
     function ResizeMapS()
     {
        if(mapS!=null)
        {
           var myMap=document.getElementById('VEMapS');
           mapS.Resize(770, 400);
           myMap.style.width = "770px";
           myMap.style.height ="400px";
        }       
     }
     
     //Show or Hide line number by check box status.
     function ShowHideLineNumS()
     {
        var chk = document.getElementById('chkShowS');
        
        if (chk != null)
        {
            showLineNumS(chk.checked);
        }
     }
      
</script>

<div id='VEMapS' class="ACA_SearchVEMapStyle">
</div>
<table role='presentation' border="0" cellpadding="0" cellspacing="0" width="770px">
    <tr>
        <td class="ACA_ARight" style="font-size: x-small; color: Gray">
            <div id="divShowOnMap">
                <input id="chkShowS" name="chkShowS" type="checkbox" onclick="javascript:FindS();HiddenAddForm();" style="vertical-align:middle" 
                title='<%= LabelUtil.RemoveHtmlFormat(GetTextByKey("caphome_map_label_show_on_map2")) %>'/>
                <span style="vertical-align:text-bottom">
                <ACA:AccelaLabel LabelKey="caphome_map_label_show_on_map2" ID="lblShowMap" 
                runat="server"></ACA:AccelaLabel></span></div>
            <input type="hidden" id="hiddenAddressS" value="" runat="server" title="" />
        </td>
    </tr>
</table>
<%} %>
