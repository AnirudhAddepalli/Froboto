using RCAPINet;
using SprayingSystem.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using SprayingSystem.Models;
using System.Diagnostics;

namespace SprayingSystem.ViewModels
{
    public class RobotVariablesViewModel : ViewModelBase
    {
        #region Fields

        private Spel _spel;
        private ComboBox cmbVars;
        private string _varValue;

        private ICommand _readValueCmd;
        private ICommand _writeValueCmd;

        private Dictionary<string, string> _variables = new Dictionary<string, string>();
        private string _variableNameSelected;

        private BioJetProcessConfig _processConfig;
        private RobotVariablesModel _varsModel;

        private Action<string> Logger;

        #endregion

        public RobotVariablesViewModel(Spel spel, RobotVariablesModel varsModel, Action<string> logger)
        {
            _spel = spel;
            _varsModel = varsModel;
            Logger = logger;

            ReadValueCmd = new RelayCommand(ReadValue, CanEdit);
            WriteValueCmd = new RelayCommand(WriteValue, CanEdit);
        }

        public ICommand ReadValueCmd
        {
            get { return _readValueCmd; }
            set
            {
                _readValueCmd = value;
                OnPropertyChanged();
            }
        }

        public ICommand WriteValueCmd
        {
            get { return _writeValueCmd; }
            set
            {
                _writeValueCmd = value;
                OnPropertyChanged();
            }
        }

        public string VariableValue
        {
            get { return _varValue;}
            set
            {
                _varValue = value;
                if (!string.IsNullOrEmpty(VariableNameSelected))
                {
                    _variables[VariableNameSelected] = _varValue;
                    _varsModel.SetValues(VariableTuples);
                }

                OnPropertyChanged();
            }
        }

        public void Initialize(Spel spel, BioJetProcessConfig process)
        {
            _spel = spel;
            _processConfig = process;

            _variables.Clear();

            _varsModel.Initialize(_variables, _processConfig);
            _varsModel.SetValues(VariableTuples);

            OnPropertyChanged("VariableNames");
        }

        public IEnumerable<string> VariableNames
        {
            get
            {
                return _variables.Keys.ToList();
            }
        }

        public string VariableNameSelected
        {
            get { return _variableNameSelected; }
            set 
            { 
                _variableNameSelected = value;
                _varValue = _variables[VariableNameSelected];
                _varsModel.SetValues(VariableTuples);
                OnPropertyChanged();
                OnPropertyChanged("VariableValue");
            }
        }

        public List<Tuple<string, string>> VariableTuples
        {
            get
            {
                var items = new List<Tuple<string, string>>();
                foreach (var variable in _variables)
                {
                    var pair = new Tuple<string, string>(variable.Key, variable.Value);
                    items.Add(pair);
                }

                return items;
            }
        }

        public void WriteToLog()
        {
            Logger("\nProcess Variables:");

            var variables = VariableTuples;
            foreach (var variable in variables)
            {
                Logger(string.Format($"{variable.Item1} = {variable.Item2}"));
            }
        }

        public override void OnConfigSettingsUpdate()
        {
            _variables.Clear();

            if (_processConfig != null)
            {
                _varsModel.Initialize(_variables, _processConfig);
                _varsModel.SetValues(VariableTuples);
            }

            OnPropertyChanged("VariableNames");
        }

        #region Private

        /*
         *  <ComboBox
         *      HorizontalAlignment="Left"
         *      Margin="31,376,0,0"
         *      VerticalAlignment="Top"
         *      Width="120"
         *      ItemsSource="{Binding Names}"/>
         */

        private bool CanEdit(object obj)
        {
            return _spel != null && _variables.Count>0;
        }

        private void ReadValue(object sender)
        {
            try
            {
                Object v;
                v = _spel.GetVar(VariableNameSelected);
                VariableValue = v.ToString();
                _varsModel.SetValues(VariableTuples);
            }
            catch (SpelException ex)
            {
                VariableValue = string.Empty;
                MessageBox.Show(ex.Message);
            }
        }

        private void WriteValue(object sender)
        {
            try
            {
                _varsModel.SetValues(VariableTuples);
                _spel.SetVar(VariableNameSelected, VariableValue);
            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion Private
    }
}
