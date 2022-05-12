# SelfHostedWebServiceSample1
This is an ASP.Net MVC Web API Self-Hosted Web Service Sample

The service can be run from the Windows desktop or in the background as a service EXE if you register it with an exe service launcher.

The service uses the classic .Net Framework, but you should also be able to build something similar in .Net Core/.Net 5/.Net 6, etc.

**Note:** You may need to run Visual Studio - ```Run As Administrator``` for the HTTP service to start as it registers a TCP port when you are testing unless you use a standard TCP/IP port. This is a Windows restriction.

# Testing the App

Start the Windows Forms App and Press Start to start monitoring port http:localhost//9999

Serving up a static web page HTML file or other asset (JPG,PNG,JS,CSS,etc.): 

http://localhost:9999  -or- http://localhost:9999/index.html

Serving up a web service route: 
http://localhost:9999/api/helloworld

```This is a sample, not a complete app, so you will need to add appropriate security measures to the app if used in a production sceanrio```
