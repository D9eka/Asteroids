using System.Threading.Tasks;
using Asteroids.Scripts.Core.InjectIds;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.SaveService
{
    public class SaveResolver : ISaveService
    {
        private readonly ISaveService _localSaveService;
        private readonly ISaveService _cloudSaveService;

        public SaveResolver(
            [Inject(Id = SaveServiceInjectId.Local)] ISaveService localSaveService, 
            [Inject(Id = SaveServiceInjectId.Cloud)] ISaveService cloudSaveService)
        {
            _localSaveService = localSaveService;
            _cloudSaveService = cloudSaveService;
        }
        public async UniTask<SaveData> Load()
        {
            var (localSaveData, cloudSaveData) = await UniTask.WhenAll(
                _localSaveService.Load(),
                _cloudSaveService.Load());
            Debug.Log($"Local saves time is {localSaveData.SaveTime}. Cloud - {cloudSaveData.SaveTime} "
                + $"use {(cloudSaveData.SaveTime >= localSaveData.SaveTime ? "cloudSaveData" : "localSaveData")}");
            return cloudSaveData.SaveTime >= localSaveData.SaveTime ? cloudSaveData : localSaveData;
        }
        public void Save(SaveData saveData)
        {
            _localSaveService.Save(saveData);
            _cloudSaveService.Save(saveData);
        }
    }
}