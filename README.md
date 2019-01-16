# Basic-WPF-Serial-IO
A basic example on how to open a serial port and write/read serial data. Useful for projects involving an Arduino/Raspberry Pi.
Can be used and then changed to fit any project or used as a guide for making your own.

# Demonstration
The data being read is simply the same text that we outputted from the write textbox. 
This means that once the Arduino receives any serial data it simply prints it back out through the serial stream.
![Alt text](https://github.com/jeremy1357/Basic-WPF-Serial-IO/blob/master/Basic_Serial_IO_Example/example.png)

You can change more of the specific port settings within the code. Kept it simple for demonstration. 

- Make sure to select the COM port that you are using to read from.
- The read/write timeout is in milliseconds. Usually keep this high. Change based on your needs.
- You can add a predefined initial/end character through the two bottom textboxes below the write box.
