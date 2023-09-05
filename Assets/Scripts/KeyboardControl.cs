using UnityEngine;
using RosSharp.RosBridgeClient.Messages.Geometry;
using RosSharp.RosBridgeClient;
using UnityEngine.UI;

public class KeyboardControl : MonoBehaviour {
    public RosConnector rosConnector;
    public string turtlebotCommandTopic = "/turtle1/cmd_vel";
    public float linearSpeed = 1f;
    public float turnSpeed = 1f;
    public RawImage up, down, left, right;

    private Twist twistMessage;
    private RosSocket rosSocket;

    private void Start() {
        rosSocket = rosConnector.RosSocket;
        twistMessage = new Twist();
    }

    private void Update() {
        if (rosConnector.Connected) {
            // Detect arrow key input
            float forwardSpeed = Input.GetAxis("Vertical") * linearSpeed;
            float angularSpeed = Input.GetAxis("Horizontal") * turnSpeed;

            // Update UI elements
            if (forwardSpeed > 0) {
                up.color = new Color(Input.GetAxis("Vertical") * 255, 0, 0);
            } else if (forwardSpeed < 0) {
                down.color = new Color(-Input.GetAxis("Vertical") * 255, 0, 0);
            } else {
                up.color = new Color(0, 0, 0);
                down.color = new Color(0, 0, 0);
            }

            if (angularSpeed > 0) {
                right.color = new Color(Input.GetAxis("Horizontal") * 255, 0, 0);
            } else if (angularSpeed < 0) {
                left.color = new Color(-Input.GetAxis("Horizontal") * 255, 0, 0);
            } else {
                right.color = new Color(0, 0, 0);
                left.color = new Color(0, 0, 0);
            }

            // Set linear and angular velocities in Twist message
            twistMessage.linear.x = forwardSpeed;
            twistMessage.angular.z = -angularSpeed;

            // Publish the Twist message to control the Turtlebot
            rosSocket.Publish(turtlebotCommandTopic, twistMessage);
        }
    }
}