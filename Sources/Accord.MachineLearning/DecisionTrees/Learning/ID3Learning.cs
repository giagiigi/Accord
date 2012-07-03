﻿// Accord Machine Learning Library
// The Accord.NET Framework
// http://accord.googlecode.com
//
// Copyright © César Souza, 2009-2012
// cesarsouza at gmail.com
//
//    This library is free software; you can redistribute it and/or
//    modify it under the terms of the GNU Lesser General Public
//    License as published by the Free Software Foundation; either
//    version 2.1 of the License, or (at your option) any later version.
//
//    This library is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//    Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public
//    License along with this library; if not, write to the Free Software
//    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
//

namespace Accord.MachineLearning.DecisionTrees.Learning
{
    using System;
    using System.Linq;
    using Accord.Math;
    using AForge;
    using Parallel = System.Threading.Tasks.Parallel;

    /// <summary>
    ///   ID3 (Iterative Dichotomiser 3) learning algorithm
    ///   for <see cref="DecisionTree">Decision Trees</see>.
    /// </summary>
    /// 
    /// <remarks>
    /// <para>
    ///   References:
    ///   <list type="bullet">
    ///     <item><description>
    ///       Quinlan, J. R 1986. Induction of Decision Trees.
    ///       Mach. Learn. 1, 1 (Mar. 1986), 81-106.</description></item>
    ///     <item><description>
    ///       Mitchell, T. M. Machine Learning. McGraw-Hill, 1997. pp. 55-58. </description></item>
    ///     <item><description><a href="http://en.wikipedia.org/wiki/ID3_algorithm">
    ///       Wikipedia, the free enclyclopedia. ID3 algorithm. Available on 
    ///       http://en.wikipedia.org/wiki/ID3_algorithm </a></description></item>
    ///   </list>
    /// </para>   
    /// </remarks>
    /// 
    /// <example>
    /// <para>
    ///   In this example, we will be using the famous Play Tennis example by Tom Mitchell (1998).
    ///   In Mitchell's example, one would like to infer if a person would play tennis or not
    ///   based solely on four input variables. Those variables are all categorical, meaning that
    ///   there is no order between the possible values for the variable (i.e. there is no order
    ///   relationship between Sunny and Rain, one is not bigger nor smaller than the other, but are 
    ///   just distinct). Moreover, the rows, or instances presented above represent days on which the
    ///   behavior of the person has been registered and annotated, pretty much building our set of 
    ///   observation instances for learning:</para>
    /// 
    /// <code>
    ///   DataTable data = new DataTable("Mitchell's Tennis Example");
    ///   
    ///   data.Columns.Add("Day", "Outlook", "Temperature", "Humidity", "Wind", "PlayTennis");
    ///   
    ///   data.Rows.Add(   "D1",   "Sunny",      "Hot",       "High",   "Weak",    "No"  );
    ///   data.Rows.Add(   "D2",   "Sunny",      "Hot",       "High",  "Strong",   "No"  ); 
    ///   data.Rows.Add(   "D3",  "Overcast",    "Hot",       "High",   "Weak",    "Yes" );
    ///   data.Rows.Add(   "D4",   "Rain",       "Mild",      "High",   "Weak",    "Yes" ); 
    ///   data.Rows.Add(   "D5",   "Rain",       "Cool",     "Normal",  "Weak",    "Yes" ); 
    ///   data.Rows.Add(   "D6",   "Rain",       "Cool",     "Normal", "Strong",   "No"  ); 
    ///   data.Rows.Add(   "D7",  "Overcast",    "Cool",     "Normal", "Strong",   "Yes" );
    ///   data.Rows.Add(   "D8",   "Sunny",      "Mild",      "High",   "Weak",    "No"  );  
    ///   data.Rows.Add(   "D9",   "Sunny",      "Cool",     "Normal",  "Weak",    "Yes" ); 
    ///   data.Rows.Add(   "D10", "Rain",        "Mild",     "Normal",  "Weak",    "Yes" ); 
    ///   data.Rows.Add(   "D11",  "Sunny",      "Mild",     "Normal", "Strong",   "Yes" );
    ///   data.Rows.Add(   "D12", "Overcast",    "Mild",      "High",  "Strong",   "Yes" ); 
    ///   data.Rows.Add(   "D13", "Overcast",    "Hot",      "Normal",  "Weak",    "Yes" ); 
    ///   data.Rows.Add(   "D14",  "Rain",       "Mild",      "High",  "Strong",   "No"  );
    /// </code>
    /// 
    /// <para>
    ///   In order to try to learn a decision tree, we will first convert this problem to a more simpler
    ///   representation. Since all variables are categories, it does not matter if they are represented
    ///   as strings, or numbers, since both are just symbols for the event they represent. Since numbers
    ///   are more easily representable than text string, we will convert the problem to use a discrete 
    ///   alphabet through the use of a <see cref="Accord.Statistics.Filters.Codification">codebook</see>.</para>
    /// 
    /// <para>
    ///   A codebook effectively transforms any distinct possible value for a variable into an integer 
    ///   symbol. For example, “Sunny” could as well be represented by the integer label 0, “Overcast” 
    ///   by “1”, Rain by “2”, and the same goes by for the other variables. So:</para>
    /// 
    /// <code>
    ///   // Create a new codification codebook to 
    ///   // convert strings into integer symbols
    ///   Codification codebook = new Codification(data);
    ///   
    ///   // Translate our training data into integer symbols using our codebook:
    ///   DataTable symbols = codebook.Apply(data); 
    ///   int[][] inputs  = symbols.ToIntArray("Outlook", "Temperature", "Humidity", "Wind"); 
    ///   int[]   outputs = symbols.ToIntArray("PlayTennis").GetColumn(0);
    /// </code>
    /// 
    /// <para>
    ///   Now that we already have our learning input/ouput pairs, we should specify our
    ///   decision tree. We will be trying to build a tree to predict the last column, entitled
    ///   “PlayTennis”. For this, we will be using the “Outlook”, “Temperature”, “Humidity” and
    ///   “Wind” as predictors (variables which will we will use for our decision). Since those
    ///   are categorical, we must specify, at the moment of creation of our tree, the
    ///   characteristics of each of those variables. So:
    /// </para>
    /// 
    /// <code>
    ///   // Gather information about decision variables
    ///   DecisionVariable[] attributes =
    ///   {
    ///     new DecisionVariable("Outlook",     3), // 3 possible values (Sunny, overcast, rain)
    ///     new DecisionVariable("Temperature", 3), // 3 possible values (Hot, mild, cool)  
    ///     new DecisionVariable("Humidity",    2), // 2 possible values (High, normal)    
    ///     new DecisionVariable("Wind",        2)  // 2 possible values (Weak, strong) 
    ///   };
    ///   
    ///   int classCount = 2; // 2 possible output values for playing tennis: yes or no
    ///
    ///   //Create the decision tree using the attributes and classes
    ///   DecisionTree tree = new DecisionTree(attributes, classCount); 
    /// </code>
    /// 
    /// <para>Now we have created our decision tree. Unfortunately, it is not really very useful,
    /// since we haven't taught it the problem we are trying to predict. So now we must instantiate
    /// a learning algorithm to make it useful. For this task, in which we have only categorical 
    /// variables, the simplest choice is to use the ID3 algorithm by Quinlan. Let’s do it:</para>
    /// 
    /// <code>
    ///   // Create a new instance of the ID3 algorithm
    ///   ID3Learning id3learning = new ID3Learning(tree);
    ///
    ///   // Learn the training instances!
    ///   id3learning.Run(inputs, outputs); 
    /// </code>
    /// 
    /// <para>The tree can now be queried for new examples through its <see cref="DecisionTree.Compute"/> method.
    /// </para>
    /// </example>
    /// 
    ///
    /// <see cref="C45Learning"/>
    /// 
    [Serializable]
    public class ID3Learning 
    {

        private DecisionTree tree;

        private IntRange[] inputRanges;
        private int outputClasses;

        private bool[] attributes;

        /// <summary>
        ///   Creates a new ID3 learning algorithm.
        /// </summary>
        /// 
        /// <param name="tree">The decision tree to be generated.</param>
        /// 
        public ID3Learning(DecisionTree tree)
        {
            this.tree = tree;
            this.inputRanges = new IntRange[tree.InputCount];
            this.outputClasses = tree.OutputClasses;

            for (int i = 0; i < inputRanges.Length; i++)
                inputRanges[i] = tree.Attributes[i].Range.ToIntRange(false);
        }

        /// <summary>
        ///   Runs the learning algorithm, creating a decision
        ///   tree modeling the given inputs and outputs.
        /// </summary>
        /// 
        /// <param name="inputs">The inputs.</param>
        /// <param name="outputs">The corresponding outputs.</param>
        /// 
        /// <returns>The error of the generated tree.</returns>
        /// 
        public double Run(int[][] inputs, int[] outputs)
        {
            attributes = new bool[tree.InputCount];

            // 1. Create a root node for the tree
            this.tree.Root = new DecisionNode(tree);

            split(tree.Root, inputs, outputs);

            // Return the classification error
            return ComputeError(inputs, outputs);
        }

        /// <summary>
        ///   Computes the prediction error for the tree
        ///   over a given set of input and outputs.
        /// </summary>
        /// 
        /// <param name="inputs">The input points.</param>
        /// <param name="outputs">The corresponding output labels.</param>
        /// 
        /// <returns>The percentual error of the prediction.</returns>
        /// 
        public double ComputeError(int[][] inputs, int[] outputs)
        {
            int miss = 0;
            for (int i = 0; i < inputs.Length; i++)
            {
                if (tree.Compute(inputs[i].ToDouble()) != outputs[i])
                    miss++;
            }

            return (double)miss / inputs.Length;
        }

        private void split(DecisionNode root, int[][] input, int[] output)
        {

            // 2. If all examples are for the same class, return the single-node
            //    tree with the output label corresponding to this common class.
            double entropy = Statistics.Tools.Entropy(output, outputClasses);

            if (entropy == 0)
            {
                if (output.Length > 0)
                    root.Output = output[0];
                return;
            }

            // 3. If number of predicting attributes is empty, then return the single-node
            //    tree with the output label corresponding to the most common value of
            //    the target attributes in the examples.
            int predictors = attributes.Count(x => x == false);

            if (predictors == 0)
            {
                root.Output = Statistics.Tools.Mode(output);
                return;
            }


            // 4. Otherwise, try to select the attribute which
            //    best explains the data sample subset.

            double[] scores = new double[predictors];
            double[] entropies = new double[predictors];
            int[][][] partitions = new int[predictors][][];

            // Retrieve candidate attribute indices
            int[] candidates = new int[predictors];
            for (int i = 0, k = 0; i < attributes.Length; i++)
                if (!attributes[i]) candidates[k++] = i;


            // For each attribute in the data set
            Parallel.For(0, scores.Length, i =>
            {
                scores[i] = computeGainRatio(input, output, candidates[i],
                    entropy, out partitions[i]);
            });

            // Select the attribute with maximum gain ratio
            int maxGainIndex; scores.Max(out maxGainIndex);
            var maxGainPartition = partitions[maxGainIndex];
            var maxGainEntropy = entropies[maxGainIndex];
            var maxGainAttribute = candidates[maxGainIndex];
            var maxGainRange = inputRanges[maxGainAttribute];

            attributes[maxGainAttribute] = true;

            // Now, create next nodes and pass those partitions as their responsabilities.
            DecisionNode[] children = new DecisionNode[maxGainPartition.Length];

            for (int i = 0; i < children.Length; i++)
            {
                children[i] = new DecisionNode(tree);
                children[i].Parent = root;
                children[i].Comparison = ComparisonKind.Equal;
                children[i].Value = i + maxGainRange.Min;


                int[][] inputSubset = input.Submatrix(maxGainPartition[i]);
                int[] outputSubset = output.Submatrix(maxGainPartition[i]);

                split(children[i], inputSubset, outputSubset); // recursion
            }

            attributes[maxGainAttribute] = false;

            root.Branches = new DecisionBranchNodeCollection(maxGainAttribute, children);
        }


        private double computeInfo(int[][] input, int[] output, int attributeIndex, out int[][] partitions)
        {
            // Compute the information gain obtained by using
            // this current attribute as the next decision node.
            double info = 0;

            IntRange valueRange = inputRanges[attributeIndex];
            partitions = new int[valueRange.Length + 1][];

            // For each possible value of the attribute
            for (int i = 0; i < partitions.Length; i++)
            {
                int value = valueRange.Min + i;

                // Partition the remaining data set
                // according to the attribute values
                partitions[i] = input.Find(x => x[attributeIndex] == value);

                // For each of the instances under responsability
                // of this node, check which have the same value
                int[] outputSubset = output.Submatrix(partitions[i]);

                // Check the entropy gain originating from this partitioning
                double e = Statistics.Tools.Entropy(outputSubset, outputClasses);

                info += ((double)outputSubset.Length / output.Length) * e;
            }

            return info;
        }

        private double computeInfoGain(int[][] input, int[] output, int attributeIndex,
            double entropy, out int[][] partitions)
        {
            return entropy - computeInfo(input, output, attributeIndex, out partitions);
        }

        private double computeGainRatio(int[][] input, int[] output, int attributeIndex,
            double entropy, out int[][] partitions)
        {
            double infoGain = computeInfoGain(input, output, attributeIndex, entropy, out partitions);
            double splitInfo = Measures.SplitInformation(output.Length, partitions);

            return infoGain == 0 ? 0 : infoGain / splitInfo;
        }
    }
}
