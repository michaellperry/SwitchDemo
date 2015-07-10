using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitchDemo
{
    public partial class VideoSwitch
    {
        public async Task MakeRouteRequest(int fromChannel, int toChannel)
        {
            await Community.AddFactAsync(new RequestRoute(
                this, Selection.Candidates, fromChannel, toChannel));
        }

        public RequestRoute NextRequest
        {
            get
            {
                // This is some example business logic. I have no
                // idea if the actual selection of requests needs to
                // be so involved. The idea here is that you should not
                // accept a request from a client that doesn't know the
                // current state.
                var currentSelection = Selection.Value;
                if (currentSelection.IsNull)
                {
                    // If I have not selected a route, choose the
                    // latest request that I've received. (Query
                    // results are in reverse chronological order.)
                    return Requests.FirstOrDefault();
                }
                else
                {
                    // If I have, choose the latest request that
                    // superscedes the requested route.
                    // Unless there are no following routes, in which
                    // case, return the current selection.
                    return currentSelection.Following.FirstOrDefault() ??
                        currentSelection;
                }
            }
        }
    }
}
