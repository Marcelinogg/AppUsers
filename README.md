HOW IS STRUCTURE OF THIS SOLUTION?:
    RRHHWeb.Client              Access to data base
    RRHHWeb.Client.Core         Model and Interfaces
    RRHHWeb.Client.Tests        Not implemented yet
    RRHHWeb.Web                 It is the API
    RRHHWeb.Web.Client          It is a program to call the API not for test else It is to the end client
    RRHHWeb.Web.Client.Tests    Not implemented yet


HOW CAN YOU EXECUTE THE API (PROJECT RRHHWeb.Web)?
    Right clic on proyect RRHHWeb.Web and clic on 'Set as StartUp Project' then clic on Start button

    When the project does not run, it probably that the next references are missing (RRHHSecurity.Core.dll, RRHHSecurity.RestClient.dll and RRHHWeb.Client.DB.dll) if this happend is needed update that dlls from folder ./Dlls


HOW TO USE THE PROJECT CLIENT (RRHHWeb.Web.Client)?
    Just compile the project after that you can create a new solucion whith a new project and add the dll reference (is locatered often in /bin/Debug/RRHHWeb.Web.Client.dll). In the documentation exists one code of example.

WHERE IS THE API?
	Server:		10.128.10.20
	IIS:		DefaultWebSite --> rrhhweb
	Service Url: http://svrsarhweb.sanborns.com.mx/rrhhweb/api/v2.0/
	User test:	2000000002 - 2000000002 (password)