# Unity_RosBridge_TurtleSim_Demo
Demonstration of controlling the ROS Turtlesim simulator with unity through RosBridge; built in Unity version 2021.3.15f1 (probably not important).

This project publishes movement commands to ```/turtle1/cmd_vel``` with the ```KeyboardControl.cs``` script.

This project subscribes to ```/turtle1/pose``` with the ```PoseSubscriber.cs``` script which uses the custom subcriber ```TurtlePose.cs``` script.

Connection to the ROS websocket is provided by EricVoll's [UWP fork](https://github.com/ericvoll/ros-sharp/tree/UWP) of the [ROS# package](https://github.com/siemens/ros-sharp) by siemens. This fork allows this project to be built to UWP devices such as the Microsoft HoloLens 2 augmented reality headset (although it won't really work since that device has no keyboard).

## Setup Instructions
1. Open the ```TurtleBotSim``` Scene in the Assets folder.
2. Download and run the Microsoft Mixed Reality Feature Tool from: https://www.microsoft.com/en-us/download/details.aspx?id=102778
3. Install the following features:
   - Mixed Reality Toolkit -> Mixed Reality Toolkit Foundation.
   - Mixed Reality Toolkit -> Mixed Reality Standard Assets .
   - Platform Support -> Mixed Reality OpenXR Plugin.
4. Go through the MRTK setup.
5. If errors persist, download NuGet for Unity .unitypackage file from: https://github.com/GlitchEnzo/NuGetForUnity/releases
6. In your Assests folder, right click --> Import Package --> Custom Package. Select the downloaded file and import it.
   - If a NuGet tab doesn’t appear on your menu bar, click the NuGet file in your Assests folder, enable Load on startup and Apply.
   - If it still doesn’t appear then restart (close and reopen) your Unity project. 

## Usage Instructions
1. On your ROS machine open two terminal windows.
2. On the first run: ```roslaunch rosbridge_server rosbridge_websocket.launch```
3. On the second run:
   - ```ifconfig``` (to get your ROS machine's IP address).
   - ```rosrun turtlesim turtlesim_node```
4. In Unity, click the "Ros Connector" GameObject and change the Ros Bridge Server URL to ```ws://<ifconfig IP Address>:9090```
5. Run the scene and use the arrow keys to make the Turtlebot move.
6. (Optional) Open a third terminal window on your ROS machine and run ```rosrun rqt_graph rqt_graph``` to see the connection between ROS and Unity.
