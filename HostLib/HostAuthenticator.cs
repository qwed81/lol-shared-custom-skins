using HostLib.Models;
using Models.Network.Messages.Client;
using Models.Network.Messages.Server;
using SharedLib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HostLib
{
    public class HostAuthenticator
    {

        public async Task<IOResult<AuthRequestData>> RecieveAuthenticationRequest(AugmentedInputStream input)
        {
            var requestResult = await input.ReadObjectAsync<AuthenticateRequest>();
            if (requestResult.Failed)
                return IOResult.CreateFailure<AuthRequestData>(requestResult.ErrorType);

            var request = requestResult.Value;
            var authData = new AuthRequestData(request.Password, request.Admin);
            return IOResult.CreateSuccess(authData);
        }

        public async Task<IOResult> SendAuthenticationResponse(AugmentedOutputStream output, Guid userId, Guid accessToken)
        {
            var response = new AuthenticateResponse(userId, accessToken);
            return await output.WriteObjectsAsync(response);
        }

    }
}
