# Load test for Photon Unity Networking

The project was created to test the behavior of PUN1 and PUN2 under load (a large count of synchronized objects).
Check photon capabilities and traffic consumption with using a different components and settings.
And compare Photon 1 with Photon 2.

This is NOT a plugin to measure your project data. But you can modificate and use it.

## Installation
The project is compatible with PUN1 and PUN2.
But it doesn't contain third party sources. So, You need:
* Install a photon yourself.
* Add scripting define symbols IS_PUN1 or IS_PUN2

## Using
### Connection
It has an simple connection. The first client, to join the server - creates a room. 
Just run it first on target device. And next on others.

### Open Load Test Window
To open the window, You need to click **Tools -> Testing -> PUN Load Test**

### Configuration
- **Object counts** - count of photon objects on scene.
- **Test time** - test execution time.
- **Loop Instant.** - forces PUN to generate new objects each time, instead of teleporting old ones.
- **Sync via RPC** - position synchronization by sending remote calls, instead of normal way. 
Sends 20 RPC per second with the coordinates of each object. Of course, this doesn't make sense in a real project.

### Run
For test start, application should be in playing mode and connected to room. 
After launch, test is performed on all clients. You can also interrupt it.

## Reports
There is extended GUI stats window, with some additional data, like current bandwidth. 
You can use it your projects, independent with test.

After end of the test, the results appear in the window, in the section "Reports".
You'll receive results from all connected clients.
Keep in mind that with a heavy load, the test may not end at the same time on master and non-master clients.
