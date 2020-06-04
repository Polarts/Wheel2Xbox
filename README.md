# Wheel2Xbox
A project that converts signals from Logitech Formula Vibration Feedback Wheel (2004 model) into virtual Xbox 360 controller signals to make it usable with modern games.
This project utilizes [AHidLib](http://ahidlib.com/pages/programming_csharp.php?lang=en) and [ScpDriverInterface](https://github.com/mogzol/ScpDriverInterface) to achieve its goal.

At this point, this project is a POC to see how far I can go with this technology.
If it proves successful, I'll consider making a generic program that allows mapping any USB HID hardware to Xbox 360 signals.

# How it works
The program is divided into 3 services. Their flow and interaction are described as a flowchart in Fig. 1.
### 1. AHid Service
Responsible for communicating with AHidLib, through which it receives report signals from the USB HID interface, zips them and forwards them into processing.
### 2. X360 Controller Service
Responsible for communicating with ScpDriverInterface, through which it reports signals to the virtual X360Controller device. The signal is built from a collection of button/axis actions it receives from Wheel Input Handler.
### 3. Wheel Input Handler
Responsible for listening to change events from AHid Service, processing them into button mappings, and forwarding them to X360 Controller Service.


![alt text](https://i.ibb.co/42N2MNZ/Annotation-2020-06-04-213453.jpg "Fig. 1: Flow chart of the software's architecture")

_Fig. 1: Flow chart of the software's architecture_


# Running locally
To run this project locally you'll need to do some setup and installations, due to its dependencies

### AHidLib
You'll need to add AHid.dll to your debug and release folders inside bin as they're not contained in this project.
You can get it by downloading [this project](http://ahidlib.com/php/download.php?lang=en&file=ahid_demo_csharp_vs17.zip) and copying the DLL from the respective folders.

### ScpDriver
You'll need to install ScpDriver on your computer.
You can get it by downloading the [latest release](https://github.com/mogzol/ScpDriverInterface/releases) of ScpDriverInterface, and installing it from the Driver Installer folder via the provided EXE.
