# Wheel2Xbox
A project that converts signals from Logitech Formula Vibration Feedback Wheel (2004 model) into virtual Xbox 360 controller signals to make it usable with modern games.
This project utilizes [HidLibrary](https://github.com/mikeobrien/HidLibrary) and [ScpDriverInterface](https://github.com/mogzol/ScpDriverInterface) to achieve its goal.

At this point, this project is a POC to see how far I can go with this technology.
If it proves successful, I'll consider making a generic program that allows mapping any USB HID hardware to Xbox 360 signals.

# How it works
The program is divided into 3 services. Their flow and interaction are described as a flowchart in Fig. 1.
### 1. Hid Service
Responsible for communicating with HidLibrary, through which it receives report signals from the USB HID interface, zips them and forwards them into processing.
### 2. X360 Controller Service
Responsible for communicating with ScpDriverInterface, through which it reports signals to the virtual X360Controller device. The signal is built from a collection of button/axis actions it receives from Wheel Input Handler.
### 3. Wheel Input Handler
Responsible for listening to change events from Hid Service, processing them into button mappings, and forwarding them to X360 Controller Service.


![alt text](https://i.ibb.co/txqyk9z/flowchart.jpg "Fig. 1: Flow chart of the software's architecture")

_Fig. 1: Flow chart of the software's architecture_


# Running locally
To run this project locally you'll need to do some setup and installations, due to its dependencies

### ScpDriver
You'll need to install ScpDriver on your computer.
You can get it by downloading the [latest release](https://github.com/mogzol/ScpDriverInterface/releases) of ScpDriverInterface, and installing it from the Driver Installer folder via the provided EXE.

# Special Thanks
- Shoutout to [Mike O'Brian](https://github.com/mikeobrien) and [Austin Mullins](https://github.com/amullins83) for developing such a wonderful cross-platform HID interface library for C#, as well as any other contributor supporting their project!
- Shoutout to [Scarlet.Crush](http://forums.pcsx2.net/User-Scarlet-Crush) for creating the SCP Virtual Bus Driver, and to [Morgan Zolob](https://github.com/mogzol) for utilizing it to make such an easy-to-use SDK for virtualizing Xbox 360 controller signals!
- A special thanks to [Rohi Ulecia](https://www.linkedin.com/in/rohi-ulecia/?fbclid=IwAR0WZMNrA071aMgfPOfnLPn17twoCt4jXVi60cqBKlhzlarLIo9VRQtPIv0) for helping me refine my algorithm and code workflow in this project :)
