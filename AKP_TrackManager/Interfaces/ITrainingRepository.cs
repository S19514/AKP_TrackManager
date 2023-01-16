using AKP_TrackManager.Models.DTO;
using AKP_TrackManager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AKP_TrackManager.Interfaces
{
    public interface ITrainingRepository
    {
        Task<IEnumerable<training>> Index(int? page, DateTime? searchString);
        Task<training> Details(int? id);
        SelectList GetLocationFriendlyNames();
        SelectList GetTrackConfigurationPresetNames();
        Task<TrainingCreateDto> Create(TrainingCreateDto training);
        Task<training> Edit(int id, training training);
        Task<bool> DeleteConfirmed(int id);
        bool trainingExists(int id);
        Task<bool> SingUpForTraining(int? id, string contextUserName);
        SelectList GetSelectedLocationFriendlyNames(int? locationId);
        SelectList GetSelectedTrackConfigurationPresetNames(int? trackConfigurationId);
    }
}
