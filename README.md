# SwitchDemo
Example Correspondence model of a system of video switches.

![Company, VideoSwitch, RequestRoute, and VideoSwitch__selection](https://raw.githubusercontent.com/michaellperry/SwitchDemo/master/Model.png)

A switch can advertise itself on the network. It creates a Company fact using a well-known identifier. Then it creates a new
VideoSwitch fact within that company. The VideoSwitch is published to the Company (hence the red arrow in the diagram above),
so that subscribers to the company receive the VideoSwitch.

A client subscribes to the Company to receive all VideoSwitch facts. It makes a request by creating a RequestRoute fact with
a pointer to the most recent requests that it knows about, and which it would like to replace. The RequestRoute is published
to the VideoSwitch so that the switch can subscribe to its own fact and receive all requests.

The switch selects a request based on the following rules:

* If it has not yet selected a request, it chooses the most recent request that was made.
* If it has selected a request, and other requests have supersceded it, it chooses the most recent among the new requests.
* If the selected request has not been supersceded, then it takes no further action.

It then creates a VideoSwitch__selection fact (which is implicitly created by setting the Selection property). Clients receive
the fact and can see the value of the Selection property.
