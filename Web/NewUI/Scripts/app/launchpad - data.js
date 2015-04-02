/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: launchpad - data.js
 * 
 *  Accela, Inc.
 *  Copyright (C): 2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: launchpad - data.js 72643 2014-06-19 09:52:06Z $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  03/18/2008     		Kevin.Feng				Initial.  
 * </pre>
 */

aca.data.launchpadstore = {
    "OfficialWebsite": "http://website.url.com",
    "GisServeruURL": "http://gis.url.com",
    "RefereshAnnouncementInMinutes": "2",
    "Modules": [
      {
          "Id": "M2",
          "Module": "Licenses",
          "SubModules": [],
          "GroupTypes": []
      },
      {
          "Id": "M3",
          "Module": "Planning",
          "SubModules": [],
          "GroupTypes": []
      },
      {
          "Id": "M5",
          "Module": "Damage Assessment",
          "SubModules": [],
          "GroupTypes": []
      },
      {
          "Id": "M6",
          "Module": "Enforcement",
          "SubModules": [],
          "GroupTypes": []
      },
      {
          "Id": "M7",
          "Module": "Permitting",
          "SubModules": [
            {
                "Id": "SM1",
                "SubModule": "Search for a..",
                "ForceLogin": false,
                "PageUrl": "/PageUrl/param{type, module}",
                "ForceTypeSelection": false
            },
            {
                "Id": "SM2",
                "SubModule": "Search for a..",
                "ForceLogin": false,
                "PageUrl": "/PageUrl/param{type, module}",
                "ForceTypeSelection": false
            }
          ],
          "GroupTypes": []
      },
      {
          "Id": "M8",
          "Module": "Asset Management",
          "SubModules": [
            {
                "Id": "SM3",
                "SubModule": "Create an Application",
                "ForceLogin": true,
                "PageUrl": "/PageUrl/param{type, module, userid, others}",
                "ForceTypeSelection": true
            },
            {
                "Id": "SM4",
                "SubModule": "Obtain a Fee Estimate",
                "ForceLogin": true,
                "PageUrl": "/PageUrl/param{type, module}",
                "ForceTypeSelection": true
            },
            {
                "Id": "SM5",
                "SubModule": "Schedule an Inspection",
                "ForceLogin": false,
                "PageUrl": "/PageUrl/param{type, module}",
                "ForceTypeSelection": false
            },
            {
                "Id": "SM6",
                "SubModule": "Search Licenses",
                "ForceLogin": false,
                "PageUrl": "/PageUrl/param{type, module}",
                "ForceTypeSelection": false
            }
          ],
          "GroupTypes": [
            {
                "Id": "GT1",
                "Type": "Public Works",
                "SubTypes": [
                  {
                      "Id": "GTS1",
                      "SubType": "Street",
                      "CategoryOrAlias": [
                        {
                            "Id": "CT1",
                            "Name": "Light Maintance"
                        },
                        {
                            "Id": "CT2",
                            "Name": "Pathhole Repair"
                        }
                      ]
                  },
                  {
                      "Id": "GTS2",
                      "SubType": "Signal",
                      "CategoryOrAlias": [
                        {
                            "Id": "CT3",
                            "Name": "Maintance"
                        },
                        {
                            "Id": "CT4",
                            "Name": "Install"
                        },
                        {
                            "Id": "CT5",
                            "Name": " Repair"
                        }
                      ]
                  }
                ]
            },
            {
                "Id": "GT2",
                "Type": "Park",
                "SubTypes": [
                  {
                      "Id": "GTS3",
                      "SubType": "Maintance",
                      "CategoryOrAlias": [
                        {
                            "Id": "CT6",
                            "Name": "NA"
                        }
                      ]
                  },
                  {
                      "Id": "GTS4",
                      "SubType": "Moving",
                      "CategoryOrAlias": [
                        {
                            "Id": "CT7",
                            "Name": "NA"
                        }
                      ]
                  }
                ]
            }
          ]
      }
    ],
    "Alerts": [
      {
          "AlertName": "Monthly Hearing Calendar"
      },
      {
          "AlertName": "My Permits"
      },
      {
          "AlertName": "Recall Notice"
      },
      {
          "AlertName": "Record in app"
      }
    ],
    "GlobalSearchIsEnabled": true,
    "GlobalSearchGroup": [
      {
          "GroupName": "Records"
      },
      {
          "GroupName": "Licensed Professionals"
      },
      {
          "GroupName": "Property Information"
      }
    ]
}

