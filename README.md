KeeStats
=============

This a plugin which computes a series of statistics on your KeePass 2.x (kdbx) password database or on a chosen group. The result is shown in a tabbed window. For more information on KeePass, check the [official website](http://www.keepass.info/). This should work with all recent 2.x KeePass versions.

Dependencies
-------

The plugin depends on:

* KeePass (of course :-)). It should work will all recent 2.x versions
* NUnit for unit tests

List of computed statistics
-----------

The current list, I'm still adding them, please contact me if you have something interesting:
* number of password
* number of groups
* empty passwords
* unique passwords
* average length for unique passwords
* Number of entries with a filled URL field
* Number of referenced passwords (REF)

Quality stats:
* shortest password
* longest password
* basic quality info (only lowercase, only uppercase...)
Single quality stats can be opened directly by clicking on the value column.

TODO
------------
Unicode not handled currently in several stats (e.g. only lowercase)
Not blocking UI when computing  + progress bar
Export stats?
Allow viewing multiple passwords (e.g. all duplicated ones)
...

KNOWN ISSUES
------------
Edit entry Cancel button not working

Unit tests
------------
NUnit tests are located in test/folder. They load a test password database (also included in the same folder) heavily based on KeePass standard database. The test so far covers only the computation part (StatComputer.cs, code coverage 100%), there is no test on the UI

Contributing
------------

