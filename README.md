# QuickIRC

The goal of this project is to create an IRC client with all of the needed features, minus a lot of noisy bells and whistles. It should be simple to use for those who just want to connect.

A secondary goal, with developers in mind, is to create readable and well-organized code. If any part of the code is difficult to grok, then please let me know that it should be improved.

## Status

QuickIRC is still being developed before its first release version. A few undesired behaviors need changed, and some interfaces need improved. There is no deadline set, since this is a hobby project.

Notable items on the todo list are:
 * Support formatted text in the messages list.
 * Improve the New Connection tab, making it much simpler to use.
 * Fill in the menu items and pretty up the tabs.
 
## Features

More features will be added, but the current feature list is:
 * All RFC 1459 and RFC 2812 commands are supported.
 * Common CTCP commands are supported, with the exception of those which allow direct connections.
 * Basic protection against denial-of-service attacks via CTCP commands.
 * Basic protection against malicious or buggy servers.
 
 
## Requirements

Microsoft Windows is required. While QuickIRC may run on earlier versions, it is only being tested on Windows 10 at this time.

## Installation

Currently, it is recommended to run the program from within Visual Studio. The `GUI` project should be set as the default project, and it should be set to run within the `out` folder.

## Bug Reporting

Please use the Issues tab on our GitHub page to file bug reports. Or, you may contact me directly if you know my contact information.
