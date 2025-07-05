using ProjectFlow.Core.Enums;
using ProjectFlow.Core.Models.DTOs.Projects.Response;
using ProjectFlow.Core.Models.Entities;
using ProjectFlow.Core.Response;

namespace ProjectFlow.ApiService.Services;

public class CommissionService
{
    private readonly Project _project;
    private readonly List<Developer> _allDevelopers;

    public CommissionService(Project project, List<Developer> allDevelopers)
    {
        _project = project;
        _allDevelopers = allDevelopers;
    }

    public Response<ProjectCommissionResponse> CalculateCommision()
    {
        var projectCommissionValue = new ProjectCommissionValue
        {
            Value = decimal.Round(_project.TotalValue * 0.4m, 2),
            TwentyPercent = decimal.Round(_project.TotalValue * 0.2m, 2),
            FifteenPercent = decimal.Round(_project.TotalValue * 0.15m, 2)
        };

        var developerCommission = _project.Developers.Count == 1 
            ? CalculateCommissionForOneDeveloper(projectCommissionValue) 
            : CalculateCommissionForTwoOrMoreDevelopers(projectCommissionValue);

        return new Response<ProjectCommissionResponse>(new ProjectCommissionResponse
        {
            ProjectId = _project.Id,
            Commission = developerCommission,
            TotalProject = _project.TotalValue,
            TotalCommission = decimal.Round(_project.TotalValue * 0.4m, 2)
        });
    }

    private List<CommissionResponse> CalculateCommissionForOneDeveloper(
        ProjectCommissionValue projectCommissionValue)
    {
        var developer = _project.Developers.First();
        var developerCommission = new List<CommissionResponse>
        {
            new()
            {
                DeveloperName = developer.Name,
                CommissionValue = developer.DevLevel == EDevLevel.Senior
                    ? projectCommissionValue.FifteenPercent
                    : projectCommissionValue.TwentyPercent
            }
        };

        projectCommissionValue.Value -= developer.DevLevel == EDevLevel.Senior
            ? projectCommissionValue.FifteenPercent
            : projectCommissionValue.TwentyPercent;

        var devRest = _allDevelopers.Count(d => d.Id != developer.Id);
        var commissionRest = decimal.Round(projectCommissionValue.Value / devRest, 2);
        foreach (var dev in _allDevelopers.Where(d => d.Id != developer.Id))
        {
            developerCommission.Add(new CommissionResponse
            {
                DeveloperName = dev.Name,
                CommissionValue = commissionRest
            });
        }

        return developerCommission;
    }

    private List<CommissionResponse> CalculateCommissionForTwoOrMoreDevelopers(
        ProjectCommissionValue projectCommissionValue)
    {
        var developerCommission = new List<CommissionResponse>();
        foreach (var dev in _project.Developers)
        {
            developerCommission.Add(new CommissionResponse
            {
                DeveloperName = dev.Name,
                CommissionValue = projectCommissionValue.FifteenPercent
            });
            projectCommissionValue.Value -= projectCommissionValue.FifteenPercent;
        }

        var devRest = _allDevelopers
            .Where(d => d.DevLevel != EDevLevel.Senior && !_project.Developers.Contains(d))
            .ToList();
        var commissionRest = decimal.Round(projectCommissionValue.Value / devRest.Count, 2);
        foreach (var dev in devRest)
        {
            developerCommission.Add(new CommissionResponse
            {
                DeveloperName = dev.Name,
                CommissionValue = commissionRest
            });
        }

        return developerCommission;
    }
}