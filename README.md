
# SunnyNet
**SunnyNet** is a C# library to allow communication with your PV system via the SunnyPortal.
## Installation
Download the [library](https://github.com/DiegoVillagrasa/SunnyNet/blob/master/SunnyNet/bin/Release/SunnyNet.dll?raw=true) and include it into your .NET project
## Usage
Create a new interface using ```SunnyNetInterface interface = new SunnyNetInterface();```
Login using your SunnyPortal credentials ```interface.Connect("username", "password");```

* Get realtime watts generated ```interface.getWatts();```

To access archived data, you'll first need to initialize the inverters ```interface.openInverter();``` and set the date of the request ```interface.setDate("day", "month", "year");```

You can then:
* Get a CSV file of the requested day ```interface.requestValuesFile("name_of_the_file");```
* Get the watts produced that day in a double array ```interface.requestValues();```

**Experimental:**
You can use ```setDateMonth()``` and ```setDateYear()``` to request data for a specified period.
## Contributing
1. Fork it!
2. Create your feature branch: `git checkout -b my-new-feature`
3. Commit your changes: `git commit -am 'Add some feature'`
4. Push to the branch: `git push origin my-new-feature`
5. Submit a pull request :D

##License
The MIT License (MIT)

Copyright (c) 2016 Diego Villagrasa

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
