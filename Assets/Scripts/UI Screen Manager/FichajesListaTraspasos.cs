using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace TacticalEleven.Scripts
{
    public class FichajesListaTraspasos
    {
        private AudioClip clickSFX;
        private MainScreen mainScreen;
        private Equipo miEquipo;
        private Manager miManager;

        private VisualElement root;

        public FichajesListaTraspasos(VisualElement rootInstance, Equipo equipo, Manager manager, MainScreen mainScreen)
        {
            root = rootInstance;
            miEquipo = equipo;
            miManager = manager;
            this.mainScreen = mainScreen;
            clickSFX = Resources.Load<AudioClip>("Audios/click");
            FechaData fechaData = new FechaData();
            fechaData.InicializarTemporadaActual();

            // Referencias a objetos de la UI
            // carteraContainer = root.Q<VisualElement>("cartera-container");
        }
    }
}