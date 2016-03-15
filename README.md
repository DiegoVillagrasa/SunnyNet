
# SunnyNet
**SunnyNet** is a C# library to allow communication with your PV system via the SunnyPortal.
## Installation
Download the library and include it into your .NET project
## Usage
Create a new interface using ```SunnyNetInterface interface = new SunnyNetInterface();```
Login using your SunnyPortal credentials ```interface.Connect("username", "password");```

* Get realtime watts generated ```interface.getWatts();```

To access archived data, you'll first need to initialize the inverters ```interface.openInverter();```
And set the date of the request ```interface.setDate("day", "month", "year");```

You can then:
* Get a CSV file of the requested day ```interface.requestValuesFile("name_of_the_file");```
* Get the watts produced that day in a double array ```interface.requestValues();```

**Experimental**
You can use ```setDateMonth``` and ```sateDateYear``` to request data for a specified period.
## Contributing
1. Fork it!
2. Create your feature branch: `git checkout -b my-new-feature`
3. Commit your changes: `git commit -am 'Add some feature'`
4. Push to the branch: `git push origin my-new-feature`
5. Submit a pull request :D
