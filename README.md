# comapre.cs
Compares files of 2 similar forks of directories.

compare.exe is a command line application that figures out the files mismatched in two similar directories.

usage:
```
compare dir1 dir2
```
It will list out the files that are present in dir1 but not in dir2, and files that are in dir2, but not in dir1.

For the common files presnet in both, it will compare the crc32 hash to figure out if they are different.
