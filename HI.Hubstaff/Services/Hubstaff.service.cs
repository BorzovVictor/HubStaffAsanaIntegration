using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using HI.SharedKernel.Models;

namespace HI.Hubstaff
{
    public class HubstaffService : IHubstaffService
    {
        private readonly HubstaffSettings _settings;

        public HubstaffService(HubstaffSettings settings)
        {
            _settings = settings;
        }

        public async Task<HsAuthModel> GenAuth()
        {
            var url = $"{_settings.BaseUrl}/auth";
            var result = await url.WithHeader("app-token", _settings.AppToken)
                .PostUrlEncodedAsync(new
                {
                    email = _settings.Email,
                    password = _settings.Password
                }).ReceiveJson<HsAuthModel>();
            _settings.AuthToken = result.User.AuthToken;
            return result;
        }

        private object GetHeaders()
        {
            return new {App_token = _settings.AppToken, Auth_Token = _settings.AuthToken};
        }

        public async Task<List<HsTaskModel>> Tasks(string projects = "", int offset = 0)
        {
            var url = _settings.BaseUrl.AppendPathSegment("tasks");

            if (!string.IsNullOrWhiteSpace(projects))
                url = url.SetQueryParam("projects", projects);
            if (offset > 0)
                url = url.SetQueryParam("offset", offset);

            var result = await url
                .WithHeaders(GetHeaders())
                .GetJsonAsync<HsTasksModel>();
            return result?.Tasks ?? new List<HsTaskModel>();
        }

        public async Task<HsTeamMemberModel> TeamByMember(HsTeamMemberRequest request)
        {
            var url = $"{_settings.BaseUrl}/custom/by_member/team";
            url = MakeParams(request, url);

            var result = await url
                .WithHeaders(GetHeaders())
                .GetJsonAsync<HsTeamMemberModel>();
            return result;
        }

        public async Task<List<HsTeamMemberTask>> GetTasksDurations(HsTeamMemberRequest request)
        {
            var data = await TeamByMember(request);
            return
                (from organization in data.Organizations
                    from user in organization.Users
                    from date in user.Dates
                    from project in date.Projects
                    from task in project.Tasks
                    select task).ToList();
        }

        string MakeParams(HsTeamMemberRequest req, string url)
        {
            if (req.StartDate == DateTime.MinValue)
                throw new Exception("start date can't be empty");
            if (req.StartDate == DateTime.MinValue)
                throw new Exception("end date can't be empty");
            url = url.SetQueryParam("start_date", req.StartDate.ToString("yyyy-MM-dd"));
            url = url.SetQueryParam("end_date", req.EndDate.ToString("yyyy-MM-dd"));
            if (!string.IsNullOrWhiteSpace(req.Organizations))
                url = url.SetQueryParam("organizations", req.Organizations.Trim());
            if (!string.IsNullOrWhiteSpace(req.Projects))
                url = url.SetQueryParam("projects", req.Projects.Trim());
            if (!string.IsNullOrWhiteSpace(req.Users))
                url = url.SetQueryParam("users", req.Users.Trim());
            if (req.ShowTasks.HasValue && req.ShowTasks == true)
                url = url.SetQueryParam("show_tasks", "true");
            if (req.ShowNotes.HasValue && req.ShowNotes == true)
                url = url.SetQueryParam("show_notes", req.ShowNotes);
            if (req.ShowActivity.HasValue && req.ShowActivity == true)
                url = url.SetQueryParam("show_activity", req.ShowActivity);
            if (req.IncludeArchived.HasValue && req.IncludeArchived == true)
                url = url.SetQueryParam("include_archived", req.IncludeArchived);

            return url;
        }
    }
}
