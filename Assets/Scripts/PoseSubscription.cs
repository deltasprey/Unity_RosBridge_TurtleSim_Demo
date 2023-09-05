using RosSharp.RosBridgeClient;
using RosSharp.RosBridgeClient.Messages.Geometry;
using UnityEngine;

public class PoseSubscription : MonoBehaviour {
    public Transform bot;
    public string botSubscribeTopic = "/turtle1/pose";

    private float x = 0, z = 0, theta = 0;
    private float oldX, oldZ, oldTheta;
    private bool initialised = false, offsetted = false;
    private RosSocket rosSocket;
    private UnityEngine.Vector3 offset, position;

    void Start() {
        rosSocket = GetComponent<RosConnector>().RosSocket;
        if (botSubscribeTopic != "") {
            // Subscribe to the ROS topic using the custom subscription message script TurtlePose
            rosSocket.Subscribe<TurtlePose>(botSubscribeTopic, poseCallback);
        }
    }

    // Update is called once per frame
    void Update() {
        if (initialised && offsetted) {
            if (x != oldX || z != oldZ || theta != oldTheta) {
                // Update GameObject transform
                position = new(x + offset.x, offset.y, z + offset.z);
                UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(90, 0, theta * Mathf.Rad2Deg - 90);
                bot.SetPositionAndRotation(position, rotation);
                oldX = x;
                oldZ = z;
                oldTheta = theta;
                //Debug.Log($"{position.x}, {position.y}, {position.z}");
            }
        } else if (initialised) {
            // Gets the offset between the initial positions of the robot and the bot in the Unity scene
            // Does not set the rotation offset
            offset = new(bot.position.x - x, bot.position.y, bot.position.z - z);
            oldX = x;
            oldZ = z;
            oldTheta = theta;
            offsetted = true;
        }
    }

    // Called when a pose message is recevied from the subscribed ROS topic
    private void poseCallback(TurtlePose msg) {
        //Debug.Log($"{msg.x}, {msg.y}, {msg.theta}, {msg.linear_velocity}, {msg.angular_velocity}");
        //print(initialised);
        x = msg.x;
        z = msg.y;
        theta = msg.theta;
        if (!initialised) {
            initialised = true;
        }
    }
}
