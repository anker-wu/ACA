Oracle Access Manager Authentication Adapter configuration steps:

1. Download "Oracle Access Manager Core Components (10.1.4.3.0) for Windows" from below URL:
   http://www.oracle.com/technetwork/middleware/ias/downloads/101401-099957.html
   http://download.oracle.com/otn/nt/middleware/11g/ofm_oam_core_win_10.1.4.3.0_disk1_1of1.zip

2. Extract and install the Access Manager SDK named "Oracle_Access_Manager10_1_4_3_0_Win32-dotnet20_AccessServerSDK.exe" from downloaded package.

3. Add reference to the "obaccess_api_mgd.dll", this dll is exists in the Access Manager SDK install directory.

4. Add the “<Access Manager SDK Install Dir> \oblix\lib” to Windows environment variable PATH.

5. Grant the application host account the Modify permission to access manager SDK install directory.

6. Enter to the command line mode and locate to the path “<Access Manager SDK Install Dir>\oblix\tools\configureAccessGate”, execute
   the configureAccessGate.exe and specify the “-t” parameter as “AccessGate” to configure the Access Manager SDK to connect to the OAM server.
   An example: configureAccessGate.exe -i “C:\Program Files\NetPoint\AccessServerSDK” -t AccessGate

7. Copy Accela.ACA.OAMAccessGate.dll/Accela.ACA.OAMAccessGate.dll.config/obaccess_api_mgd.dll to ACA bin folder and configure the Accela.ACA.OAMAccessGate.dll.config.

8. Turn on “Enable 32-Bit Applications” for ACA application pool if IIS server is x64.

9. Modify the “Bll.config” in “Config” folder in the ACA application folder, set IAuthAdapter interface as below:
   <object id="IAuthAdapter" type="Accela.ACA.OAMAccessGate.OAMAuthAdapter, Accela.ACA.OAMAccessGate" singleton="true" ></object>

10. Set the HTTP handler according to the descriptions in ObrarHttpHandler.cs file.

Remarks:
1. May be you will meet the "missing msvcr70.dll/msvci70.dll" error, please reinstall the "Oracle_Access_Manager10_1_4_3_0_Win32-dotnet20_AccessServerSDK.exe" or copy these DLLs from <Project Dir>\lib\ to %WinDir%\system32\ and try again.
2. "Error 5226, Invalid argument. Culture is not supported. Parameter name: name. Neutral is an invalid culture identifier."
	This project developed in VS 2010, if you installed VS 2012 in your PC, you will meet this error when you compile the project. Please another PC without VS2012.
3. "Could not load file or assembly 'obaccess_api_mgd.DLL' or one of its dependencies. The specified module could not be found."
	If you meet this issue, please add OAM SDK install path to system PATH environment, and set the folder permission to allow the hosted account of ACA application pool to modify it.
4. "Could not load file or assembly 'obaccess_api_mgd' or one of its dependencies. An attempt was made to load a program with an incorrect format."
	May be you do not trun on "Enable 32-Bit Applications" for ACA application pool.
5. If you cannot login to OAM, and saw the following error message in OAM server log(/home/alan/Oracle/Middleware/user_projects/domains/OAMDomain/servers/oam_server1/logs/oam_server1.out), may be the user password is expired.
	"<Apr 24, 2014 7:07:57 PM CST> <Error> <oracle.oam.user.identity.provider> <OAMSSA-20023> <Authentication Failure for user : alan.hu.>"
	You can follow the following steps to change the password:
		a. Open the web page: http://alan-oam.alanoim.com:14000/oim/faces/pages/Login.jspx
		b. Click "Forgot Password" link.
		c. Enter your "User Login" and click Next button.
		d. Answer the Challenge Questions and click Next button. For the administrator account (xelsysadm) and "alan.hu", the Questions and Answers as below:
			mather's name:cdx
			born city:yl
			liks color:black
		e. Enter "New Password" and "Confirm New Password" and click Save button

Dev environment configurations (see the "OAM DEV environment.txt"):

1. Configure Access Gate:
	 Please enter the Mode in which you want the AccessGate to run : 1(Open) 2(Simple) 3(Cert) : 1
	 Please enter the AccessGate ID : WebGate10g
	 Please enter the Password for this AccessGate : taotao0426
	 Please enter the Access Server ID : alan-oam.alanoim.com
	 Please enter the Access Server Host Machine Name : alan-oam.alanoim.com
	 Please enter the Access Server Port : 5575
	 Preparing to connect to Access Server.  Please wait.
	 AccessGate installed Successfully.
	 Press enter key to continue ...

2. Configure the authentication policy:
Use weblogic/taotao0426 log in http://alan-oam.alanoim.com:7001/oamconsole --> Authentication Schemes --> OID11G(ALAN-OEL)AuthScheme

App.config for development environment:
--------------------------------------------------------
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <configSections>
    <section name="OAMConfiguration" type="Accela.ACA.OAMAccessGate.OAMConfiguration,Accela.ACA.OAMAccessGate"/>
  </configSections>

  <!--
  ASDKInstallDir - The Access Manager SDK install directory without the "/" or "\" suffix.
      For example: C:\Program Files\NetPoint\AccessServerSDK
  HostIdentifier -  The Host Identifier defined in OAM server to indicates the HTTP resources.
  ChallengeURL - It's should point to the virtual path of the "obrareq.cgi" in OAM server.
      For example: https://oam.denver.gov/oam/server/obrareq.cgi
  -->
  <OAMConfiguration
    ASDKInstallDir="D:\Program Files (x86)\Oracle\AccessServerSDK"
    HostIdentifier="WebGate10g"
    ChallengeURL="https://alan-oam.alanoim.com:14101/oam/server/obrareq.cgi"
    RegisterURL="https://alan-oam.alanoim.com:14001/oim/faces/pages/USelf.jspx?E_TYPE=USELF&OP_TYPE=SELF_REGISTRATION"
    LogoutURL="https://alan-oam.alanoim.com:14101/oam/server/logout">

    <!--
    AttributesMapping  -  A mapping table between ACA user and external user in order to synchronize data from the third party server.
                          Administrator needs to configure the "extname" attribute to specify the Response Name of the Authorization Policy.
                          The Authorization Policy and policy Response are defined in Access Manager Console, and the Response Type must be Header.
                          "maxlength" is the maximum length of the fields in Accela Automation database.
                          Notes: Do not to change the name, dbname and maxlength attributes if you do not understand the function of those properties.
    -->
    <AttributesMapping>
      <Attribute name="Email"         dbname="email"         maxlength="80"   extname="OAM_USER_EMAIL"/>
      <Attribute name="FirstName"     dbname="firstName"     maxlength="70"   extname="OAM_USER_FIRSTNAME"/>
      <Attribute name="MiddleName"    dbname="middleName"    maxlength="70"   extname=""/>
      <Attribute name="LastName"      dbname="lastName"      maxlength="70"   extname="OAM_USER_LASTNAME"/>
      <Attribute name="Gender"        dbname="gender"        maxlength="1"    extname=""/>
      <Attribute name="Salutation"    dbname="salutation"    maxlength="255"  extname=""/>
      <Attribute name="BirthDate"     dbname="birthDate"     maxlength=""     extname="OAM_USER_BIRTHDAY"/>
      <Attribute name="Address"       dbname="compactAddress.addressLine1"       maxlength="200"  extname="OAM_USER_ADDRESS1"/>
      <Attribute name="Address2"      dbname="compactAddress.addressLine2"      maxlength="200"  extname="OAM_USER_ADDRESS2"/>
      <Attribute name="City"          dbname="compactAddress.city"          maxlength="30"   extname=""/>
      <Attribute name="State"         dbname="compactAddress.state"         maxlength="30"   extname=""/>
      <Attribute name="Country"       dbname="country"       maxlength="30"   extname=""/>
      <Attribute name="ZIP"           dbname="compactAddress.zip"           maxlength="10"   extname="OAM_USER_ZIP"/>
      <Attribute name="MobilePhone"   dbname="phone2"     maxlength="40"   extname="OAM_USER_MOBILE"/>
      <Attribute name="HomePhone"     dbname="phone1"     maxlength="40"   extname=""/>
      <Attribute name="WorkPhone"     dbname="phone3"     maxlength="40"   extname="OAM_USER_PHONE"/>
      <Attribute name="Fax"           dbname="fax"           maxlength="40"   extname="OAM_USER_FAX"/>
      <Attribute name="FEIN"          dbname="fein"          maxlength="16"   extname=""/>
      <Attribute name="SSN"           dbname="maskedSsn"     maxlength="11"   extname=""/>
      <Attribute name="BusinessName"  dbname="businessName"  maxlength="255"  extname=""/>
      <Attribute name="Title"         dbname="title"     maxlength="255"  extname="OAM_USER_TITLE"/>
      <Attribute name="P.O.Box"       dbname="postOfficeBox"         maxlength="30"   extname=""/>
    </AttributesMapping>
  </OAMConfiguration>
</configuration>
--------------------------------------------------------