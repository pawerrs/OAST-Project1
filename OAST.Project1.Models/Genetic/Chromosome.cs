﻿using System;
using System.Collections.Generic;

namespace OAST.Project1.Models.Genetic
{
    public abstract class Chromosome
    {
        public int Fitness { get; set; }

        public abstract List<Chromosome> Crossover(Chromosome chromosomeToCrossWith, Random random);

        public abstract Chromosome Mutate(Random random);

        public abstract Chromosome Clone();
    }
}
