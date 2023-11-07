using BlazorAuthDemo.Models;
using BlazorAuthDemo.Models.Requests;
using BlazorAuthDemo.Models.Responses;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Abstractions;
using Microsoft.Identity.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorAuthDemo.Views.Components
{
    public partial class AuthenticatedUser
    {
        [Inject]
        public IDownstreamApi DownstreamApi { get; set; }

        [Inject]
        public ILogger<AuthenticatedUser> Logger { get; set; }

        [Inject]
        public MicrosoftIdentityConsentAndConditionalAccessHandler ConsentHandler { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        public AuthenticatedUserComponentState State { get; set; }
        public User User { get; set; }

        private const string RequestIdForUserDetails = "1";
        private const string RequestUrlForUserDetails =
            "/me?$select=displayName,mail";

        private const string RequestIdForUserGroups = "2";
        private const string RequestUrlForUserGroups = 
            "/me/memberOf?$select=displayName";

        protected async override Task OnInitializedAsync()
        {
            try
            {
                this.State = AuthenticatedUserComponentState.Loading;
                var batchRequest = GenerateRequest();

                BatchResponse response =
                    await DownstreamApi
                        .PostForUserAsync<BatchRequest, BatchResponse>(
                            "MicrosoftGraph",
                            batchRequest,
                            options => options.RelativePath = "$batch");

                User = ExtractUserDataFromBatchResponse(response);
                User.Groups = ExtractUserGroupsFromBatchResponse(response);

                this.State = AuthenticatedUserComponentState.Content;
            }
            catch (MicrosoftIdentityWebChallengeUserException ex)
            {
                ConsentHandler.HandleException(ex);
            }
            catch (Exception exception)
            {
                Logger.LogError(exception.Message);
                this.State = AuthenticatedUserComponentState.Error;
            }
        }

        private static BatchRequest GenerateRequest()
        {
            return new BatchRequest
            {
                Requests = new List<Request>
                    {
                        new Request
                        {
                            Id = RequestIdForUserDetails,
                            Method = Method.GET,
                            Url = RequestUrlForUserDetails
                        },
                        new Request
                        {
                            Id = RequestIdForUserGroups,
                            Method = Method.GET,
                            Url = RequestUrlForUserGroups
                        }
                    }
            };
        }

        private static User ExtractUserDataFromBatchResponse(
            BatchResponse batchResponse)
        {
            User userData = null;

            object body = (batchResponse.Responses
                        .Where(response =>
                                RequestIdForUserDetails.Equals(response.Id) &&
                                response.Status == 200)
                        .FirstOrDefault()?
                        .Body) ??
                        throw new Exception(
                            "Error processing user data request in Graph API");

            userData =
                    JsonSerializer.Deserialize<User>(
                        body.ToString(),
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

            return userData;
        }

        private static List<string> ExtractUserGroupsFromBatchResponse(
            BatchResponse batchResponse)
        {
            List<string> userGroups = null;

            object body = (batchResponse.Responses
                        .Where(response =>
                                RequestIdForUserGroups.Equals(response.Id) &&
                                response.Status == 200)
                        .FirstOrDefault()?
                        .Body) ??
                        throw new Exception(
                            "Error processing user groups request in Graph API");

            UserGroupResponse userGroupResponse =
                JsonSerializer.Deserialize<UserGroupResponse>(
                    body.ToString(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            userGroups = userGroupResponse.Value
                            .Select(group => group.DisplayName)
                            .ToList();

            return userGroups;
        }
    }

    public enum AuthenticatedUserComponentState
    {
        Loading,
        Content,
        Error
    }
}