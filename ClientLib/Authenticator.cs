using ClientLib.Models;
using Models.Network;
using Models.Network.Messages.Client;
using Models.Network.Messages.Server;
using SharedLib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClientLib
{
    public class Authenticator
    {

        public async Task<IOResult> SendAuthenticationRequest(AugmentedOutputStream output,
            string password, bool admin)
        {
            var request = new AuthenticateRequest(admin, password);
            return await output.WriteObjectsAsync(request);
        }

        public async Task<IOResult<ClientInfo>> RecieveAuthenticationInfo(AugmentedInputStream input)
        {
            var responseResult = await input.ReadObjectAsync<AuthenticateResponse>();
            if (responseResult.Failed)
                return IOResult.CreateFailure<ClientInfo>(responseResult.ErrorType);

            var response = responseResult.Value;
            var clientInfo = new ClientInfo(response.UserId, response.PrivateAccessToken);
            
            return IOResult.CreateSuccess(clientInfo);
        }

    }
}
