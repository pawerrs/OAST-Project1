using System;
using System.Diagnostics;
using OAST.Project1.Models.Genetic;
using System.Threading.Tasks;
using OAST.Project1.Common.Enums;
using OAST.Project1.DataAccess.ConfigHandlers;
using OAST.Project1.Models.Common;

namespace OAST.Project1.Services.Genetic
{
    public class GeneticService
    {
        private readonly FileParser _fileParser;
        private readonly FileName _fileName;
        private readonly ProblemType _problemType;
        private readonly GeneticAlgorithmParameters _parameters;

        public GeneticService(MenuOptions menuOptions)
        {
            _parameters = menuOptions.GeneticAlgorithmParameters;
            _fileName = menuOptions.FileName;
            _problemType = menuOptions.ProblemType;

            _fileParser = new FileParser();
        }

        public void Solve()
        {
            Action fitnessFunction = null;

            switch (_problemType)
            {
                case ProblemType.DDAP:
                    fitnessFunction = EvaluateDDAPSolution;
                    break;
                case ProblemType.DAP:
                    fitnessFunction = EvaluateDAPSolution;
                    break;
            }

            OptimizeWithGeneticAlgorithm(fitnessFunction);
        }

        private void OptimizeWithGeneticAlgorithm(Action fitnessFunction)
        {
            var status = new GeneticAlgorithmState
            {
                ElapsedTime = Stopwatch.StartNew(),
                NumberOfGenerations = 0,
                NumberOfGenerationsWithoutImprovement = 0,
                NumberOfMutations = 0
            };
        }

        private void EvaluateDDAPSolution()
        {


        }

        private void EvaluateDAPSolution()
        {

        }

        private void GenerateInitialPopulation()
        {

        }

        private bool EvaluateStoppingCriteria(GeneticAlgorithmState state) =>
            _parameters.StoppingCriteria switch
            {
                StoppingCriteria.ElapsedTime => state.ElapsedTime.ElapsedMilliseconds / 1000 >= _parameters.LimitValue,
                StoppingCriteria.NoImprovement => state.NumberOfGenerationsWithoutImprovement >= _parameters.LimitValue,
                StoppingCriteria.NumberOfGenerations => state.NumberOfGenerations >= _parameters.LimitValue,
                StoppingCriteria.NumberOfMutations => state.NumberOfGenerations >= _parameters.LimitValue,
                _ => false
            };
    }
}
