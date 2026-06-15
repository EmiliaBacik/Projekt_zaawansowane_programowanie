using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using System.Xml.Xsl;
using System.Diagnostics.Metrics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Text;
using Microsoft.VisualBasic.Logging;
using System.Diagnostics;
using static System.Formats.Asn1.AsnWriter;
using System.Text.Json;

namespace Projekt_zaawansowane_metody_obliczeniowe
{
    public partial class Form1 : Form
    {
        volatile bool Active = true;
        volatile bool Stop = false;
        //BackgroundWorker bw;
        private Stopwatch stoper = new Stopwatch();

        static string seq_opt = "";
        Random rnd = new Random();
        List<string> Instance = new List<String> { };

        //PARAMETRY
        int population_size = 100;
        int iteration_cycles_count = 200;
        double mutation_probability = 0.0003;
        int number_of_extension_of_the_child = 2;
        double percent_of_parents_left_in_the_population = 0.25;
        double reproductive_pressure = 15.0;
        bool automatic_change_of_mutation_probability = true;
        int downtime_length = 20;
        double tmp_mutation_probability = 0.004;

        //dane do testow
        public class TestInstance
        {
            public int M { get; set; }
            public int N { get; set; }
            public int NSeqOpt { get; set; }
            public int ErrCount { get; set; }
            public string GeneratorSolution { get; set; }
            public List<string> Instance { get; set; }
        }

        class GAResult
        {
            public string BestSolution = "";
            public int BestValue;
            public long TimeMs;
        }

        private List<TestInstance> generatedTests = new List<TestInstance>();

        public Form1()
        {
            InitializeComponent();
            this.FormClosing += Form1_FormClosing;
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Stop = true;
        }

        private (List<string> Instance_local, string SeqOpt) GenerateInstance(int m, int n, int n_seq_opt, int err_count)
        {
            List<string> Instance_local = new List<string>();
            List<char> nucleotides = new List<char> { 'A', 'C', 'G', 'T' };
            List<char> seq = new List<char>();

            for (int j = 0; j < n_seq_opt; j++)
            {
                int los = rnd.Next(0, 4);
                seq.Add(nucleotides[los]);
            }

            string seq_opt_local = new string(seq.ToArray());

            for (int i = 0; i < m; i++)
            {
                List<char> seq1 = new List<char>();

                for (int j = 0; j < n_seq_opt; j++)
                    seq1.Add(seq[j]);

                for (int j = 0; j < n_seq_opt - n; j++)
                {
                    int new_n = seq1.Count;
                    int los = rnd.Next(0, new_n);
                    seq1.RemoveAt(los);
                }

                Instance_local.Add(new string(seq1.ToArray()));
            }

            List<List<int>> changelog = new List<List<int>>();

            for (int i = 0; i < err_count; i++)
            {
                int los_seq = rnd.Next(0, Instance_local.Count);
                int type_of_error = rnd.Next(0, 2);

                if (i == err_count - 1 || type_of_error == 0)
                {
                    int id_to_change = rnd.Next(0, n);
                    char[] seq_with_error = Instance_local[los_seq].ToCharArray();
                    int los = rnd.Next(0, 4);
                    seq_with_error[id_to_change] = nucleotides[los];
                    string string_seq = new string(seq_with_error);
                    List<int> check_id = new List<int> { los_seq, id_to_change };
                    if (string_seq == Instance_local[los_seq] || changelog.Contains(check_id))
                    {
                        err_count++;
                    }
                    else
                    {
                        Instance_local[los_seq] = string_seq;
                        changelog.Add(new List<int> { los_seq, id_to_change });
                    }
                }
                else
                {
                    int id_to_change = rnd.Next(0, n - 1);
                    char[] seq_with_error = Instance_local[los_seq].ToCharArray();
                    char tmp = seq_with_error[id_to_change];
                    seq_with_error[id_to_change] = seq_with_error[id_to_change + 1];
                    seq_with_error[id_to_change + 1] = tmp;
                    string string_seq = new string(seq_with_error);
                    List<int> check_id1 = new List<int> { los_seq, id_to_change };
                    List<int> check_id2 = new List<int> { los_seq, id_to_change + 1 };

                    if (string_seq == Instance_local[los_seq] ||
                        changelog.Contains(check_id1) ||
                        changelog.Contains(check_id2))
                    {
                        err_count++;
                    }
                    else
                    {
                        err_count--;
                        Instance_local[los_seq] = string_seq;
                        changelog.Add(new List<int> { los_seq, id_to_change });
                        changelog.Add(new List<int> { los_seq, id_to_change + 1 });
                    }
                }
            }
            return (Instance_local, seq_opt_local);
        }

        private List<string> GeneratePopulation(List<string> Instance_local)
        {
            List<string> Population = new List<string>();
            for (int i = 0; i < population_size; i++)
            {
                List<int> counter = Enumerable.Repeat(0, Instance_local.Count).ToList();
                List<char> entity = new List<char>();

                while (counter.Any(x => x < Instance_local[0].Length))
                {
                    int los = rnd.Next(0, Instance_local.Count);
                    if (counter[los] >= Instance_local[los].Length)
                        continue;
                    char nucleotide = Instance_local[los][counter[los]];
                    entity.Add(nucleotide);

                    for (int j = 0; j < Instance_local.Count; j++)
                    {
                        if (counter[j] < Instance_local[j].Length &&
                            Instance_local[j][counter[j]] == nucleotide)
                        {
                            counter[j]++;
                        }
                    }
                }
                Population.Add(new string(entity.ToArray()));
            }
            return Population;
        }

        private GAResult RunGeneticAlgorithm(List<string> Instance_local)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            List<string> wPopulation = GeneratePopulation(Instance_local);
            double w_mutation_probability = mutation_probability;
            HashSet<string> Best_results = new HashSet<string>();
            Best_results.Add(wPopulation[0]);
            int best_value = wPopulation[0].Length;

            for (int j = 0; j < wPopulation.Count; j++)
            {
                if (wPopulation[j].Length < best_value)
                {
                    best_value = wPopulation[j].Length;
                    Best_results.Clear();
                    Best_results.Add(wPopulation[j].Trim());
                }
                else if (wPopulation[j].Length == best_value)
                {
                    Best_results.Add(wPopulation[j].Trim());
                }
            }
            List<double> points_history = new List<double>();
            for (int i = 0; i <= iteration_cycles_count; i++)
            {
                List<string> Parents = new List<string>(wPopulation);
                wPopulation.Clear();
                int staying_alive = (int)Math.Round(Parents.Count * percent_of_parents_left_in_the_population, MidpointRounding.AwayFromZero);

                for (int j = 0; j < staying_alive; j++)
                    wPopulation.Add(SelectParent(Parents));

                while (wPopulation.Count < population_size)
                {
                    string los1 = SelectParent(Parents);
                    string los2 = SelectParent(Parents);
                    if (los1 == los2)
                        continue;
                    string child = Reproduction(Instance_local, los1, los2);
                    if (child != "e")
                        wPopulation.Add(child);
                }
                int population_total_length = 0;
                for (int j = 0; j < wPopulation.Count; j++)
                    population_total_length += wPopulation[j].Length;

                //sprawdzenie czy nowe najlepsze rozwi¹zanie
                for (int j = 0; j < wPopulation.Count; j++)
                {
                    if (wPopulation[j].Length < best_value)
                    {
                        Best_results.Clear();
                        best_value = wPopulation[j].Length;
                        Best_results.Add(wPopulation[j].Trim());
                    }
                    else if (wPopulation[j].Length == best_value && !Best_results.Contains(wPopulation[j]))
                        Best_results.Add(wPopulation[j].Trim());
                }

                //sprawdzenie czy przestoj
                if (automatic_change_of_mutation_probability)
                {
                    bool is_downtime = false;
                    var points = points_history;
                    if (points.Count >= downtime_length)
                    {
                        var last_ten = points.Skip(points.Count - downtime_length).ToList();
                        double first_value_y = last_ten[0];
                        is_downtime = last_ten.All(p => p == first_value_y);

                        if (is_downtime)
                            w_mutation_probability = tmp_mutation_probability;
                    }
                }

                int number_of_mutation = (int)Math.Round(population_total_length * w_mutation_probability, MidpointRounding.AwayFromZero);

                for (int j = 0; j < number_of_mutation; j++)
                {
                    int los = rnd.Next(0, wPopulation.Count);
                    int los2 = rnd.Next(0, wPopulation[los].Length);
                    List<int> counter = Enumerable.Repeat(0, Instance_local.Count).ToList();

                    for (int k = 0; k < los2; k++)
                    {
                        for (int l = 0; l < Instance_local.Count; l++)
                        {
                            if (counter[l] < Instance_local[l].Length &&
                                wPopulation[los][k] == Instance_local[l][counter[l]])
                            {
                                counter[l]++;
                            }
                        }
                    }

                    var NotFullInstance_locals =
                        counter.Select((val, idx) => new
                        {
                            Wartosc = val,
                            Indeks = idx
                        })
                        .Where(x => x.Wartosc != Instance_local[x.Indeks].Length)
                        .Select(x => x.Indeks)
                        .ToList();

                    if (NotFullInstance_locals.Count == 0)
                    {
                        j--;
                        continue;
                    }

                    int los3 = NotFullInstance_locals[rnd.Next(0, NotFullInstance_locals.Count)];

                    if (counter[los3] >= Instance_local[los3].Length)
                    {
                        j--;
                        continue;
                    }

                    if (Instance_local[los3][counter[los3]] == wPopulation[los][los2])
                    {
                        j--;
                        continue;
                    }

                    wPopulation[los] = wPopulation[los].Insert(los2, Instance_local[los3][counter[los3]].ToString());
                    wPopulation[los] = SimplifyIndividual(wPopulation[los], Instance_local);
                }
                if (automatic_change_of_mutation_probability)
                    w_mutation_probability = mutation_probability;

                //sprawdzenie najlepszej wartosci
                for (int j = 0; j < wPopulation.Count; j++)
                {
                    if (wPopulation[j].Length < best_value)
                    {
                        Best_results.Clear();
                        best_value = wPopulation[j].Length;
                        Best_results.Add(wPopulation[j].Trim());
                    }
                    else if (wPopulation[j].Length == best_value)
                    {
                        Best_results.Add(wPopulation[j].Trim());
                    }
                }
                points_history.Add(best_value);
            }

            sw.Stop();

            return new GAResult
            {
                BestValue = best_value,
                BestSolution = Best_results.First(),
                TimeMs = sw.ElapsedMilliseconds
            };
        }

        private void SaveCsv(int m, int n, int err_count, List<string> seq_opt, List<GAResult> results)
        {
            //MessageBox.Show("Próbujê zapisaæ do wyniki_testow.csv i srednie_wyniki.csv");
            string fileName = "wyniki_testow2.csv";
            StringBuilder sb = new StringBuilder();

            string downtimeLengthValue =
                automatic_change_of_mutation_probability
                ? downtime_length.ToString()
                : "-";

            string tmpMutationProbabilityValue =
                automatic_change_of_mutation_probability
                ? tmp_mutation_probability.ToString()
                : "-";

            if (!File.Exists(fileName))
            {
                sb.AppendLine(
                    "m;n;err_count;population_size;iteration_cycles_count;" +
                    "mutation_probability;percent_of_parents_left_in_the_population;" +
                    "reproductive_pressure;number_of_extension_of_the_child;" +
                    "automatic_change_of_mutation_probability;" +
                    "downtime_length;" +
                    "tmp_mutation_probability;" +
                    "seq_opt;dlugosc_seq_opt;BestValue;BestSolution;TimeMs");
            }

            int seqOptLength = seq_opt != null ? seq_opt[0].Length : 0;

            for (int i = 0; i < results.Count; i++)
            {
                sb.AppendLine(
                    $"{m};" +
                    $"{n};" +
                    $"{err_count};" +
                    $"{this.population_size};" +
                    $"{this.iteration_cycles_count};" +
                    $"{this.mutation_probability};" +
                    $"{this.percent_of_parents_left_in_the_population};" +
                    $"{this.reproductive_pressure};" +
                    $"{this.number_of_extension_of_the_child};" +
                    $"{automatic_change_of_mutation_probability};" +
                    $"{downtimeLengthValue};" +
                    $"{tmpMutationProbabilityValue};" +
                    $"{seq_opt[i]};" +
                    $"{seqOptLength};" +
                    $"{results[i].BestValue};" +
                    $"{results[i].BestSolution};" +
                    $"{results[i].TimeMs}");
            }

            File.AppendAllText(fileName, sb.ToString());

            if (results == null || results.Count == 0) return;

            string avgFileName = "srednie_wyniki2.csv";
            StringBuilder sbAvg = new StringBuilder();

            if (!File.Exists(avgFileName))
            {
                sbAvg.AppendLine(
                     "m;n;err_count;population_size;iteration_cycles_count;" +
                     "mutation_probability;percent_of_parents_left_in_the_population;" +
                     "reproductive_pressure;number_of_extension_of_the_child;" +
                     "automatic_change_of_mutation_probability;" +
                     "downtime_length;" +
                     "tmp_mutation_probability;" +
                     "dlugosc_seq_opt;SredniaBestValue;SredniaTimeMs");
            }

            // Wyliczanie œrednich za pomoc¹ LINQ
            double avgBestValue = results.Average(r => r.BestValue);
            double avgTimeMs = results.Average(r => r.TimeMs);

            sbAvg.AppendLine(
                $"{m};" +
                $"{n};" +
                $"{err_count};" +
                $"{this.population_size};" +
                $"{this.iteration_cycles_count};" +
                $"{this.mutation_probability};" +
                $"{this.percent_of_parents_left_in_the_population};" +
                $"{this.reproductive_pressure};" +
                $"{this.number_of_extension_of_the_child};" +
                $"{automatic_change_of_mutation_probability};" +
                $"{downtimeLengthValue};" +
                $"{tmpMutationProbabilityValue};" +
                $"{seqOptLength};" +
                $"{avgBestValue};" +
                $"{avgTimeMs}");

            File.AppendAllText(avgFileName, sbAvg.ToString());
            MessageBox.Show($"Zapisano plik w: {Path.GetFullPath(fileName)} i {Path.GetFullPath(avgFileName)}");
        }

        public static string FindLCS(string s1, string s2)
        {
            if (string.IsNullOrEmpty(s1) || string.IsNullOrEmpty(s2))
            {
                return string.Empty;
            }

            int m = s1.Length;
            int n = s2.Length;
            int[,] dp = new int[m + 1, n + 1];
            for (int i = 1; i <= m; i++)
            {
                for (int j = 1; j <= n; j++)
                {
                    if (s1[i - 1] == s2[j - 1])
                        dp[i, j] = dp[i - 1, j - 1] + 1;
                    else
                        dp[i, j] = Math.Max(dp[i - 1, j], dp[i, j - 1]);
                }
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            int x = m, y = n;
            while (x > 0 && y > 0)
            {
                if (s1[x - 1] == s2[y - 1])
                {
                    sb.Append(s1[x - 1]); // Znak pasuje
                    x--;
                    y--;
                }
                else if (dp[x - 1, y] >= dp[x, y - 1])
                    x--;
                else
                    y--;
            }

            //Odwrócenie wyniku
            char[] array = sb.ToString().ToCharArray();
            Array.Reverse(array);

            return new string(array);
        }

        public static (string Subsequence, List<int> IndicesS1, List<int> IndicesS2) ZnajdzLcsZPozycjami(string s1, string s2)
        {
            if (string.IsNullOrEmpty(s1) || string.IsNullOrEmpty(s2))
                return (string.Empty, new List<int>(), new List<int>());

            int m = s1.Length;
            int n = s2.Length;
            int[,] dp = new int[m + 1, n + 1];

            for (int i = 1; i <= m; i++)
            {
                for (int j = 1; j <= n; j++)
                {
                    if (s1[i - 1] == s2[j - 1])
                    {
                        dp[i, j] = dp[i - 1, j - 1] + 1;
                    }
                    else
                    {
                        dp[i, j] = Math.Max(dp[i - 1, j], dp[i, j - 1]);
                    }
                }
            }

            List<int> indicesS1 = new List<int>();
            List<int> indicesS2 = new List<int>();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            int x = m, y = n;

            while (x > 0 && y > 0)
            {
                if (s1[x - 1] == s2[y - 1])
                {
                    sb.Append(s1[x - 1]); // Znak pasuje

                    // Zapisywanie indeksów
                    indicesS1.Add(x - 1);
                    indicesS2.Add(y - 1);

                    x--;
                    y--;
                }
                else if (dp[x - 1, y] >= dp[x, y - 1])
                    x--;
                else
                    y--;
            }

            //Odwrócenie wyniku
            char[] charArray = sb.ToString().ToCharArray();
            Array.Reverse(charArray);
            string lcsString = new string(charArray);

            indicesS1.Reverse();
            indicesS2.Reverse();

            return (lcsString, indicesS1, indicesS2);
        }

        private string SimplifyIndividual_old(string individual, List<string> Instance)
        {
            StringBuilder child = new StringBuilder(individual);
            List<int> counter = Enumerable.Repeat(0, Instance.Count).ToList();
            for (int i = 0; i < child.Length; i++)
            {
                for (int j = 0; j < Instance.Count; j++)
                {
                    if (counter[j] < Instance[j].Length && child[i] == Instance[j][counter[j]])
                        counter[j]++;
                }

                // wszystkie sekwencje ju¿ pokryte
                if (!counter.Any(x => x < Instance[0].Length))
                {
                    child.Length = i + 1;
                    break;
                }
            }
            return child.ToString();
        }

        private string SimplifyIndividual(string individual, List<string> Instance)
        {
            StringBuilder child = new StringBuilder(individual);
            List<int> counter = Enumerable.Repeat(0, Instance.Count).ToList();
            for (int i = 0; i < child.Length; i++)
            {
                bool nuc_used = false;
                for (int j = 0; j < Instance.Count; j++)
                {
                    if (counter[j] < Instance[j].Length && child[i] == Instance[j][counter[j]])
                    {
                        counter[j]++;
                        nuc_used = true;
                    }
                }

                // nukleotyd nie zosta³ wykorzystany przez ¿adn¹ sekwencjê
                if (!nuc_used)
                {
                    child.Remove(i, 1);
                    i--;
                    continue;
                }

                // wszystkie sekwencje ju¿ pokryte
                if (!counter.Any(x => x < Instance[0].Length))
                {
                    child.Length = i + 1;
                    break;
                }
            }

            return child.ToString();
        }

        private string Reproduction(List<string> Instance_local, string seq1, string seq2)
        {
            var wynik = ZnajdzLcsZPozycjami(seq1, seq2);
            string subsequence = wynik.Subsequence;
            List<int> indicesS1 = wynik.IndicesS1;
            List<int> indicesS2 = wynik.IndicesS2;
            System.Text.StringBuilder child = new System.Text.StringBuilder();

            int currentIdxS1 = 0;
            int currentIdxS2 = 0;

            for (int i = 0; i < subsequence.Length; i++)
            {
                int lcsIdxS1 = indicesS1[i];
                int lcsIdxS2 = indicesS2[i];
                string gapS1 = seq1.Substring(currentIdxS1, lcsIdxS1 - currentIdxS1);
                string gapS2 = seq2.Substring(currentIdxS2, lcsIdxS2 - currentIdxS2);
                if (rnd.Next(0, 2) == 0)
                    child.Append(gapS1);
                else
                    child.Append(gapS2);
                child.Append(subsequence[i]);
                currentIdxS1 = lcsIdxS1 + 1;
                currentIdxS2 = lcsIdxS2 + 1;
            }

            string finalGapS1 = seq1.Substring(currentIdxS1);
            string finalGapS2 = seq2.Substring(currentIdxS2);

            if (rnd.Next(0, 2) == 0)
                child.Append(finalGapS1);
            else
                child.Append(finalGapS2);
            string old_child = child.ToString();


            List<int> counter = Enumerable.Repeat(0, Instance_local.Count).ToList();
            List<char> entity = new List<char> { };
            for (int i = 0; i < child.Length; i++)
            {
                if (!counter.Any(x => x < Instance_local[0].Length))
                {
                    child.Length = i;
                    break;
                }
                bool nuc_used = false;
                for (int j = 0; j < Instance_local.Count; j++)
                {
                    if (counter[j] < Instance_local[j].Length && child[i] == Instance_local[j][counter[j]])
                    {
                        counter[j]++;
                        nuc_used = true;
                    }
                }
                if (!nuc_used)
                {
                    // Czy istnieje kolejny znak i czy by³by on przydatny
                    if (i + 1 < child.Length)
                    {
                        char nextChar = child[i + 1];
                        bool next_would_be_used = false;

                        for (int j = 0; j < Instance_local.Count; j++)
                        {
                            if (counter[j] < Instance_local[j].Length && nextChar == Instance_local[j][counter[j]])
                            {
                                next_would_be_used = true;
                                break;
                            }
                        }

                        if (next_would_be_used)
                        {
                            child.Remove(i, 1);
                            i--;
                            continue;
                        }
                    }

                    var NotFullInstance_locals = counter.Select((val, idx) => new { Wartosc = val, Indeks = idx })
                        .Where(x => x.Wartosc != Instance_local[x.Indeks].Length)
                        .Select(x => x.Indeks)
                        .ToList();
                    int los = NotFullInstance_locals[rnd.Next(0, NotFullInstance_locals.Count)];
                    child.Insert(i, Instance_local[los][counter[los]]);
                    i--;
                    if (i > 0)
                        i--;
                    else
                        i = -1;
                }
            }
            List<int> counter2 = Enumerable.Repeat(0, Instance_local.Count).ToList();
            for (int l = 0; l < child.Length; l++)
                for (int j = 0; j < Instance_local.Count; j++)
                {
                    if (counter2[j] < Instance_local[j].Length && child[l] == Instance_local[j][counter2[j]])
                        counter2[j]++;
                }
            if (counter2.Any(x => x < Instance_local[0].Length))
            {
                for (int i = 0; i < number_of_extension_of_the_child; i++)
                {
                    if (counter2.Any(x => x < Instance_local[0].Length))
                    {
                        var NotFullInstance_locals = counter2.Select((val, idx) => new { Wartosc = val, Indeks = idx })
                        .Where(x => x.Wartosc != Instance_local[x.Indeks].Length)
                        .Select(x => x.Indeks)
                        .ToList();
                        int los = NotFullInstance_locals[rnd.Next(0, NotFullInstance_locals.Count)];
                        child.Insert(child.Length, Instance_local[los][counter2[los]]);
                        for (int j = 0; j < Instance_local.Count; j++)
                            if (counter2[j] < Instance_local[j].Length && child[child.Length - 1] == Instance_local[j][counter2[j]])
                                counter2[j]++;
                    }
                    else
                        return child.ToString();
                }
                return "e";
            }
            return child.ToString();
        }

        private string SelectParent(List<string> Parents)
        {
            var ranked = Parents.OrderBy(x => x.Length).ToList();
            int n = ranked.Count;
            List<double> weights = new List<double>();

            for (int i = 0; i < n; i++)
            {
                double weight;
                if (n == 1)
                    weight = 1.0;
                else
                    weight = reproductive_pressure - (reproductive_pressure - 1.0) * i / (n - 1);

                weights.Add(weight);
            }
            double totalWeight = weights.Sum();
            double r = rnd.NextDouble() * totalWeight;
            double cumulative = 0;

            for (int i = 0; i < n; i++)
            {
                cumulative += weights[i];
                if (r <= cumulative)
                    return ranked[i];
            }
            return ranked[n - 1];
        }

        private void button1_Click(object sender, EventArgs e) //generuj instancje
        {
            Instance.Clear();
            seq_opt = "";
            int m;
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                m = rnd.Next(2, 50);
            }
            else
            {
                if (int.TryParse(textBox1.Text, out m) && m>0) { }
                else
                {
                    MessageBox.Show("To nie jest poprawna liczba dodatnia ca³kowita!");
                    textBox1.Focus();
                    return;
                }
            }
            int n, n_seq_opt, err_count;
            bool if_n_random = false;
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                if (!string.IsNullOrWhiteSpace(textBox4.Text))
                {
                    MessageBox.Show("Je¿eli podajesz d³ugoœæ sekwencji optymalnej, podaj te¿ d³ugoœæ sekwencji w instancji!");
                    textBox4.Focus();
                    return;
                }
                else
                    n = rnd.Next(5, 80);
                if_n_random = true;
            }
            else
            {
                if (int.TryParse(textBox2.Text, out n) && n>0) { }
                else
                {
                    MessageBox.Show("To nie jest poprawna liczba dodatnia ca³kowita!");
                    textBox2.Focus();
                    return;
                }
            }
            if (string.IsNullOrWhiteSpace(textBox4.Text))
            {
                n_seq_opt = rnd.Next(n, n + 50);
            }
            else
            {
                if (int.TryParse(textBox4.Text, out n_seq_opt) && n_seq_opt>0) { }
                else
                {
                    MessageBox.Show("To nie jest poprawna liczba dodatnia ca³kowita!");
                    textBox4.Focus();
                    return;
                }
                if (if_n_random == false && n_seq_opt < n)
                {
                    MessageBox.Show("D³ugoœæ sekwencji optymalnej musi byæ wiêksza lub równa d³ugoœci sekwencji!");
                    textBox4.Focus();
                    return;
                }
            }
            if (string.IsNullOrWhiteSpace(textBox5.Text))
            {
                err_count = rnd.Next(0, 3 * m);
            }
            else
            {
                if (int.TryParse(textBox5.Text, out err_count) && err_count>=0) { }
                else
                {
                    MessageBox.Show("To nie jest poprawna liczba ca³kowita wiêksza ni¿ 0!");
                    textBox5.Focus();
                    return;
                }
            }
            /*
            List<char> nucleotides = new List<char> { 'A', 'C', 'G', 'T' };
            List<char> seq = new List<char> { };
            for (int j = 0; j < n_seq_opt; j++)
            {
                int los = rnd.Next(0, 4);
                seq.Add(nucleotides[los]);
            }
            seq_opt = new string(seq.ToArray());
            //MessageBox.Show(seq_opt + " dlugosc to: " + seq.Count.ToString());

            for (int i = 0; i < m; i++)
            {
                List<char> seq1 = new List<char> { };
                for (int j = 0; j < n_seq_opt; j++)
                {
                    seq1.Add(seq[j]);
                }
                for (int j = 0; j < n_seq_opt - n; j++)
                {
                    int new_n = seq1.Count;
                    int los = rnd.Next(0, new_n);
                    seq1.RemoveAt(los);
                }
                var string_seq = new string(seq1.ToArray());
                Instance.Add(string_seq);
            }
            List<List<int>> changelog = new List<List<int>> { };
            for (int i = 0; i < err_count; i++)
            {
                int los_seq = rnd.Next(0, Instance.Count);
                int type_of_error = rnd.Next(0, 2);
                if (i == err_count - 1 || type_of_error == 0)
                {
                    int id_to_change = rnd.Next(0, n);
                    char[] seq_with_error = Instance[los_seq].ToCharArray();
                    int los = rnd.Next(0, 4);
                    seq_with_error[id_to_change] = nucleotides[los];
                    var string_seq = new string(seq_with_error.ToArray());
                    List<int> check_id = new List<int> { los_seq, id_to_change };
                    if (string_seq == Instance[los_seq] || changelog.Contains(check_id))
                        err_count++;
                    else
                    {
                        Instance[los_seq] = string_seq;
                        List<int> id_of_changed = new List<int> { los_seq, id_to_change };
                        changelog.Add(id_of_changed);
                    }
                }
                else
                {
                    int id_to_change = rnd.Next(0, n - 1);
                    char[] seq_with_error = Instance[los_seq].ToCharArray();
                    char tmp = seq_with_error[id_to_change];
                    seq_with_error[id_to_change] = seq_with_error[id_to_change + 1];
                    seq_with_error[id_to_change + 1] = tmp;
                    var string_seq = new string(seq_with_error.ToArray());
                    List<int> check_id1 = new List<int> { los_seq, id_to_change };
                    List<int> check_id2 = new List<int> { los_seq, id_to_change + 1 };
                    if (string_seq == Instance[los_seq] || changelog.Contains(check_id1) || changelog.Contains(check_id2))
                        err_count++;
                    else
                    {
                        err_count--;
                        Instance[los_seq] = string_seq;
                        List<int> id_of_changed1 = new List<int> { los_seq, id_to_change };
                        List<int> id_of_changed2 = new List<int> { los_seq, id_to_change + 1 };
                        changelog.Add(id_of_changed1);
                        changelog.Add(id_of_changed2);
                    }
                }
            }
            */
            var dane = GenerateInstance(m, n, n_seq_opt, err_count);
            Instance = dane.Instance_local;
            seq_opt = dane.SeqOpt;
            string matrix_text = string.Join(Environment.NewLine, Instance);
            textBox3.WordWrap = false;
            textBox3.Text = matrix_text;
            button3.Enabled = true;
            textBox16.Text = seq_opt;
            label23.Text = seq_opt.Length.ToString();
            textBox16.Visible = true;
            label23.Visible = true;
            label20.Visible = true;
            label22.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e) //sprawdŸ instancje
        {
            Instance.Clear();
            bool correct_instance = true;
            var linie = textBox3.Lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToList();
            if (linie.Count == 0) correct_instance = false;
            int first_n = linie[0].Length;
            string nucleotides = "AGCT";
            correct_instance = correct_instance && linie.All(l =>
                l.Length == first_n &&
                l.All(znak => nucleotides.Contains(znak))
            );
            if (!correct_instance)
            {
                MessageBox.Show("Wprowadzono niepoprawn¹ instancjê!");
                return;
            }
            else
            {
                label2.Visible = true;
                button3.Enabled = true;
                Instance = textBox3.Lines.ToList();
                textBox16.Text = "Nie dotyczy";
                label23.Text = "Nie dotyczy";
                textBox16.Visible = true;
                label23.Visible = true;
                label20.Visible = true;
                label22.Visible = true;
            }

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            button3.Enabled = false;
            label2.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrWhiteSpace(textBox10.Text))
            {
                if (int.TryParse(textBox10.Text, out population_size)) { }
                else
                {
                    MessageBox.Show("To nie jest poprawna liczba ca³kowita!");
                    textBox10.Focus();
                    return;
                }
            }
            if (!string.IsNullOrWhiteSpace(textBox11.Text))
            {
                if (int.TryParse(textBox11.Text, out iteration_cycles_count)) { }
                else
                {
                    MessageBox.Show("To nie jest poprawna liczba ca³kowita!");
                    textBox11.Focus();
                    return;
                }
            }
            if (!string.IsNullOrWhiteSpace(textBox12.Text))
            {
                if (double.TryParse(textBox12.Text, out mutation_probability)) { }
                else
                {
                    MessageBox.Show("To nie jest poprawna liczba u³amkowa z zakresu 0-1!");
                    textBox12.Focus();
                    return;
                }
                if (mutation_probability > 1 || mutation_probability < 0)
                {
                    MessageBox.Show("To nie jest poprawna liczba u³amkowa z zakresu 0-1!");
                    mutation_probability = 0.01;
                    textBox12.Focus();
                    return;
                }
            }
            if (!string.IsNullOrWhiteSpace(textBox13.Text))
            {
                if (double.TryParse(textBox13.Text, out percent_of_parents_left_in_the_population)) { }
                else
                {
                    MessageBox.Show("To nie jest poprawna liczba u³amkowa z zakresu 0-1!");
                    textBox13.Focus();
                    return;
                }
                if (percent_of_parents_left_in_the_population > 1 || percent_of_parents_left_in_the_population < 0)
                {
                    MessageBox.Show("To nie jest poprawna liczba u³amkowa z zakresu 0-1!");
                    percent_of_parents_left_in_the_population = 0.2;
                    textBox13.Focus();
                    return;
                }
            }
            if (!string.IsNullOrWhiteSpace(textBox14.Text))
            {
                if (double.TryParse(textBox14.Text, out reproductive_pressure)) { }
                else
                {
                    MessageBox.Show("To nie jest poprawna liczba u³amkowa wiêksza ni¿ 1!");
                    textBox14.Focus();
                    return;
                }
                if (reproductive_pressure < 1)
                {
                    MessageBox.Show("To nie jest poprawna liczba u³amkowa wiêksza ni¿ 1!");
                    reproductive_pressure = 2.0;
                    textBox14.Focus();
                    return;
                }
            }
            if (!string.IsNullOrWhiteSpace(textBox15.Text))
            {
                if (int.TryParse(textBox15.Text, out number_of_extension_of_the_child)) { }
                else
                {
                    MessageBox.Show("To nie jest poprawna liczba ca³kowita!");
                    textBox15.Focus();
                    return;
                }
            }

            if (checkBox1.Checked)
            {
                if (!string.IsNullOrWhiteSpace(textBox17.Text))
                {
                    if (double.TryParse(textBox17.Text, out tmp_mutation_probability)) { }
                    else
                    {
                        MessageBox.Show("To nie jest poprawna liczba ca³kowita!");
                        textBox17.Focus();
                        return;
                    }
                }
                if (!string.IsNullOrWhiteSpace(textBox18.Text))
                {
                    if (int.TryParse(textBox18.Text, out downtime_length)) { }
                    else
                    {
                        MessageBox.Show("To nie jest poprawna liczba ca³kowita!");
                        textBox18.Focus();
                        return;
                    }
                }

            }
            else
                automatic_change_of_mutation_probability = false;

            Stop = false;   // W¹tek ma prawo dzia³aæ
            Active = true;  // W¹tek nie jest zapauzowany na starcie
            stoper.Reset();
            stoper.Start();

            buttonPause.Text = "Pause";
            button3.Visible = false;
            buttonStop.Visible = true;
            buttonPause.Visible = true;
            textBox6.Visible = true;
            label8.Visible = true;
            label25.Visible = true;
            textBox7.Visible = true;
            label9.Visible = true;
            textBox8.Visible = true;
            label10.Visible = true;
            textBox9.Visible = true;
            label11.Visible = true;
            button1.Enabled = false;
            button2.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;

            chart1.Series.Clear();
            chart1.Titles.Clear();
            chart1.Visible = true;
            var seria = chart1.Series.Add("Funkcja celu");
            seria.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            seria.BorderWidth = 2;
            var obszarWykresu = chart1.ChartAreas[0];
            obszarWykresu.AxisX.MajorGrid.LineColor = Color.LightGray;
            obszarWykresu.AxisY.MajorGrid.LineColor = Color.LightGray;
            obszarWykresu.AxisX.Title = "Liczba iteracji (pokoleñ)";
            obszarWykresu.AxisY.Title = "Wartoœæ funkcji celu";
            var tytul = chart1.Titles.Add("Wykres zmian funkcji celu w czasie dzia³ania algorytmu");
            tytul.Font = new Font("Segoe UI", 9, FontStyle.Bold);

            //generowanie populacji pocz¹tkowej
            /*
            List<string> Population = new List<string>();
            for (int i = 0; i < population_size; i++)
            {
                List<int> counter = Enumerable.Repeat(0, Instance.Count).ToList();
                List<char> entity = new List<char> { };
                while (counter.Any(x => x < Instance[0].Length))
                {
                    int los = rnd.Next(0, Instance.Count);
                    if (counter[los] >= Instance[los].Length)
                        continue;
                    char nucleotide = Instance[los][counter[los]];
                    entity.Add(nucleotide);
                    for (int j = 0; j < Instance.Count; j++)
                    {
                        if (counter[j] < Instance[j].Length && Instance[j][counter[j]] == nucleotide)
                            counter[j]++;
                    }
                }
                var string_entity = new string(entity.ToArray());
                Population.Add(string_entity);
            }*/
            List<string> Population = new List<string>();
            Population = GeneratePopulation(Instance);
            textBox6.Text = string.Join(Environment.NewLine, Population);


            Stop = false;
            BackgroundWorker bw = new BackgroundWorker();
            bw.WorkerReportsProgress = true;

            // kod wykonywany na osobnym w¹tku
            bw.DoWork += new DoWorkEventHandler(
                delegate (object o, DoWorkEventArgs args)
                {
                    BackgroundWorker b = o as BackgroundWorker;
                    List<string> wPopulation = new List<string>((List<string>)args.Argument);
                    double w_mutation_probability = mutation_probability;

                    List<double> points_history = new List<double>();
                    HashSet<string> Best_results = new HashSet<string>();
                    Best_results.Add(wPopulation[0]);
                    int best_value = wPopulation[0].Length;
                    //sprawdzenie najlepszego rozwi¹zania
                    for (int j = 0; j < wPopulation.Count; j++)
                    {
                        if (wPopulation[j].Length < best_value)
                        {
                            best_value = wPopulation[j].Length;
                            Best_results.Add(wPopulation[j].Trim());
                        }
                        else if (wPopulation[j].Length == best_value && !Best_results.Contains(wPopulation[j]))
                            Best_results.Add(wPopulation[j].Trim());
                    }

                    for (int i = 0; i <= iteration_cycles_count; i++)
                    {
                        // zatrzymanie po zamkniêciu okna
                        if (Stop)
                        {
                            args.Cancel = true;
                            return;
                        }

                        // pauza
                        while (!Active)
                        {
                            //Thread.Sleep(200);

                            if (Stop)
                            {
                                args.Cancel = true;
                                return;
                            }
                        }

                        //wybor osobnikow do krzyzowania
                        List<string> Parents = new List<string>(wPopulation);
                        //zabijanie rodziców
                        wPopulation.Clear();
                        int staying_alive = (int)Math.Round(Parents.Count * percent_of_parents_left_in_the_population, MidpointRounding.AwayFromZero);
                        for (int j = 0; j < staying_alive; j++)
                            wPopulation.Add(SelectParent(Parents));
                        //reprodukcja
                        while (wPopulation.Count < population_size)
                        {
                            string los1 = SelectParent(Parents);
                            string los2 = SelectParent(Parents);
                            if (los1 == los2)
                                continue;
                            string child = Reproduction(Instance, los1, los2);
                            if (child != "e")
                                wPopulation.Add(child);
                        }

                        //sprawdzenie czy nowe najlepsze rozwi¹zanie
                        for (int j = 0; j < wPopulation.Count; j++)
                        {
                            if (wPopulation[j].Length < best_value)
                            {
                                Best_results.Clear();
                                best_value = wPopulation[j].Length;
                                Best_results.Add(wPopulation[j].Trim());
                            }
                            else if (wPopulation[j].Length == best_value && !Best_results.Contains(wPopulation[j]))
                                Best_results.Add(wPopulation[j].Trim());
                        }

                        //sprawdzenie czy przestoj
                        if (automatic_change_of_mutation_probability)
                        {
                            bool is_downtime = false;
                            var points = points_history;
                            if (points.Count >= downtime_length)
                            {
                                var last_ten = points.Skip(points.Count - downtime_length).ToList();
                                double first_value_y = last_ten[0];
                                is_downtime = last_ten.All(p => p == first_value_y);

                                if (is_downtime)
                                    w_mutation_probability = tmp_mutation_probability;
                            }
                        }

                        //mutacje
                        int population_total_length = 0;
                        for (int j = 0; j < wPopulation.Count; j++)
                            population_total_length += Population[j].Length;
                        int number_of_mutation = (int)Math.Round(population_total_length * w_mutation_probability, MidpointRounding.AwayFromZero);
                        //MessageBox.Show("Liczba mutacji to: " + number_of_mutation.ToString());
                        for (int j = 0; j < number_of_mutation; j++)
                        {
                            int los = rnd.Next(0, wPopulation.Count);
                            int los2 = rnd.Next(0, wPopulation[los].Length);
                            List<int> counter = Enumerable.Repeat(0, Instance.Count).ToList();
                            for (int k = 0; k < los2; k++)
                            {
                                for (int l = 0; l < Instance.Count; l++)
                                {
                                    if (counter[l] < Instance[l].Length && wPopulation[los][k] == Instance[l][counter[l]])
                                        counter[l]++;
                                }
                            }
                            var NotFullInstances = counter.Select((val, idx) => new { Wartosc = val, Indeks = idx })
                                .Where(x => x.Wartosc != Instance[x.Indeks].Length)
                                .Select(x => x.Indeks)
                                .ToList();
                            if (NotFullInstances.Count == 0)
                            {
                                j--;
                                continue;
                            }
                            int los3 = NotFullInstances[rnd.Next(0, NotFullInstances.Count)];
                            if (Instance[los3][counter[los3]] == wPopulation[los][los2] || counter[los3] >= Instance[los3].Length)
                            {
                                j--;
                                continue;
                            }
                            string old_entity = wPopulation[los];
                            wPopulation[los] = wPopulation[los].Insert(los2, Instance[los3][counter[los3]].ToString());
                            wPopulation[los] = SimplifyIndividual(wPopulation[los], Instance);
                        }
                        if (automatic_change_of_mutation_probability)
                            w_mutation_probability = mutation_probability;

                        //sprawdzenie czy nowe najlepsze rozwi¹zanie
                        for (int j = 0; j < wPopulation.Count; j++)
                        {
                            if (wPopulation[j].Length < best_value)
                            {
                                Best_results.Clear();
                                best_value = wPopulation[j].Length;
                                Best_results.Add(wPopulation[j].Trim());
                            }
                            else if (wPopulation[j].Length == best_value && !Best_results.Contains(wPopulation[j]))
                                Best_results.Add(wPopulation[j].Trim());
                        }

                        //przekazanie paczki
                        string[] guiData = new string[]
                        {
                            string.Join(Environment.NewLine, wPopulation),
                            string.Join(Environment.NewLine, Best_results),
                            best_value.ToString(),
                            stoper.Elapsed.ToString(@"mm\:ss"),
                            i.ToString(),
                };

                        // Oblicz procent postêpu
                        int progressPercent = (int)((double)i / iteration_cycles_count * 100);

                        // Raportujemy postêp i przesy³amy tablicê do w¹tku g³ównego
                        b.ReportProgress(progressPercent, guiData);
                        points_history.Add(best_value);

                        //Thread.Sleep(500);
                    }
                    stoper.Stop();
                });

            // BEZPIECZNA AKTUALIZACJA GUI
            bw.ProgressChanged += new ProgressChangedEventHandler(
                delegate (object o, ProgressChangedEventArgs args)
                {
                    if (args.UserState is string[] data)
                    {
                        textBox6.Text = data[0]; // CurrentPopulation
                        textBox7.Text = data[1]; // BestResultsText
                        textBox8.Text = data[2]; // BestValueText

                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine(data[1].Split(Environment.NewLine)[0]);
                        foreach (string seq in Instance)
                        {
                            int seqIdx = 0;
                            for (int i = 0; i < data[1].Split(Environment.NewLine)[0].Length; i++)
                            {
                                if (seqIdx < seq.Length && seq[seqIdx] == data[1].Split(Environment.NewLine)[0][i])
                                {
                                    sb.Append(seq[seqIdx]);
                                    seqIdx++;
                                }
                                else
                                {
                                    sb.Append(' ');
                                }
                            }
                            sb.AppendLine();
                        }
                        textBox9.Text = sb.ToString();
                        label12.Text = data[3];
                        if (data.Length >= 5 && double.TryParse(data[4], out double xIteracja) && double.TryParse(data[2], out double yFunkcjaCelu))
                        {
                            chart1.Series["Funkcja celu"].Points.AddXY(xIteracja, yFunkcjaCelu);
                        }
                    }
                    label7.Text = $"Postêp: {args.ProgressPercentage}%";
                });

            // zakoñczenie obliczeñ
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
                delegate (object o, RunWorkerCompletedEventArgs args)
                {
                    stoper.Stop();
                    button3.Visible = true;
                    buttonStop.Visible = false;
                    buttonPause.Visible = false;
                    button1.Enabled = true;
                    button2.Enabled = true;
                    button4.Enabled = true;
                    if (generatedTests.Count > 0)
                    {
                        button5.Enabled = true;
                        button6.Enabled = true;
                    }
                    button7.Enabled = true;

                    if (args.Error != null)
                    {
                        MessageBox.Show($"W¹tek wywali³ siê z powodu b³êdu:\n{args.Error.Message}\n\nMiejsce b³êdu:\n{args.Error.StackTrace}");
                        label7.Text = "B³¹d krytyczny";
                    }
                    else if (args.Cancelled)
                    {
                        label7.Text = "Przerwano";
                    }
                    else
                    {
                        label7.Text = "Zakoñczono";
                    }
                });

            // start nowego w¹tku
            bw.RunWorkerAsync(Population);
        }

        // przycisk Pause/Resume
        private void buttonPause_Click(object sender, EventArgs e)
        {
            if (Active)
            {
                Active = false;
                buttonPause.Text = "Wznów";
                stoper.Stop();
            }
            else
            {
                Active = true;
                buttonPause.Text = "Pauza";
                stoper.Start();

            }
        }

        // przycisk Stop
        private void buttonStop_Click(object sender, EventArgs e)
        {
            Stop = true;
            stoper.Stop();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button4.Enabled = false;
            //List<GAResult> results = new List<GAResult>();
            //List<string> opt_seqs = new List<string>();
            int testsCount = 10;
            int m, n, n_seq_opt, err_count;
            bool if_n_random = false;
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                m = rnd.Next(2, 50);
            }
            else
            {
                if (int.TryParse(textBox1.Text, out m) && m > 0) { }
                else
                {
                    MessageBox.Show("To nie jest poprawna liczba dodatnia ca³kowita!");
                    textBox1.Focus();
                    button4.Enabled = true;
                    button5.Enabled = true;
                    button6.Enabled = true;
                    return;
                }
            }
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                if (!string.IsNullOrWhiteSpace(textBox4.Text))
                {
                    MessageBox.Show("Je¿eli podajesz d³ugoœæ sekwencji optymalnej, podaj te¿ d³ugoœæ sekwencji w instancji!");
                    textBox4.Focus();
                    button4.Enabled = true;
                    button5.Enabled = true;
                    button6.Enabled = true;
                    return;
                }
                else
                    n = rnd.Next(5, 80);
                if_n_random = true;
            }
            else
            {
                if (int.TryParse(textBox2.Text, out n) && n > 0) { }
                else
                {
                    MessageBox.Show("To nie jest poprawna liczba dodatnia ca³kowita!");
                    textBox2.Focus();
                    button4.Enabled = true;
                    button5.Enabled = true;
                    button6.Enabled = true;
                    return;
                }
            }
            if (string.IsNullOrWhiteSpace(textBox4.Text))
            {
                n_seq_opt = rnd.Next(n, n + 50);
            }
            else
            {
                if (int.TryParse(textBox4.Text, out n_seq_opt) && n_seq_opt > 0) { }
                else
                {
                    MessageBox.Show("To nie jest poprawna liczba dodatnia ca³kowita!");
                    textBox4.Focus();
                    button4.Enabled = true;
                    button5.Enabled = true;
                    button6.Enabled = true;
                    return;
                }
                if (if_n_random == false && n_seq_opt < n)
                {
                    MessageBox.Show("D³ugoœæ sekwencji optymalnej musi byæ wiêksza lub równa d³ugoœci sekwencji!");
                    textBox4.Focus();
                    button4.Enabled = true;
                    button5.Enabled = true;
                    button6.Enabled = true;
                    return;
                }
            }
            if (string.IsNullOrWhiteSpace(textBox5.Text))
            {
                err_count = rnd.Next(0, 3 * m);
            }
            else
            {
                if (int.TryParse(textBox5.Text, out err_count) && err_count >= 0) { }
                else
                {
                    MessageBox.Show("To nie jest poprawna liczba ca³kowita wiêksza ni¿ 0!");
                    textBox5.Focus();
                    button4.Enabled = true;
                    button5.Enabled = true;
                    button6.Enabled = true;
                    return;
                }
            }
            generatedTests.Clear();

            for (int i = 0; i < testsCount; i++)
            {
                var generated = GenerateInstance(m, n, n_seq_opt, err_count);

                generatedTests.Add(
                    new TestInstance
                    {
                        M = m,
                        N = n,
                        NSeqOpt = n_seq_opt,
                        ErrCount = err_count,
                        GeneratorSolution = generated.SeqOpt,
                        Instance = generated.Instance_local
                    });
            }

            MessageBox.Show(
                $"Wygenerowano {generatedTests.Count} instancji.");

            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrWhiteSpace(textBox10.Text))
            {
                if (int.TryParse(textBox10.Text, out population_size)) { }
                else
                {
                    MessageBox.Show("To nie jest poprawna liczba ca³kowita!");
                    textBox10.Focus();
                    return;
                }
            }
            if (!string.IsNullOrWhiteSpace(textBox11.Text))
            {
                if (int.TryParse(textBox11.Text, out iteration_cycles_count)) { }
                else
                {
                    MessageBox.Show("To nie jest poprawna liczba ca³kowita!");
                    textBox11.Focus();
                    return;
                }
            }
            if (!string.IsNullOrWhiteSpace(textBox12.Text))
            {
                if (double.TryParse(textBox12.Text, out mutation_probability)) { }
                else
                {
                    MessageBox.Show("To nie jest poprawna liczba u³amkowa z zakresu 0-1!");
                    textBox12.Focus();
                    return;
                }
                if (mutation_probability > 1 || mutation_probability < 0)
                {
                    MessageBox.Show("To nie jest poprawna liczba u³amkowa z zakresu 0-1!");
                    mutation_probability = 0.01;
                    textBox12.Focus();
                    return;
                }
            }
            if (!string.IsNullOrWhiteSpace(textBox13.Text))
            {
                if (double.TryParse(textBox13.Text, out percent_of_parents_left_in_the_population)) { }
                else
                {
                    MessageBox.Show("To nie jest poprawna liczba u³amkowa z zakresu 0-1!");
                    textBox13.Focus();
                    return;
                }
                if (percent_of_parents_left_in_the_population > 1 || percent_of_parents_left_in_the_population < 0)
                {
                    MessageBox.Show("To nie jest poprawna liczba u³amkowa z zakresu 0-1!");
                    percent_of_parents_left_in_the_population = 0.2;
                    textBox13.Focus();
                    return;
                }
            }
            if (!string.IsNullOrWhiteSpace(textBox14.Text))
            {
                if (double.TryParse(textBox14.Text, out reproductive_pressure)) { }
                else
                {
                    MessageBox.Show("To nie jest poprawna liczba u³amkowa wiêksza ni¿ 1!");
                    textBox14.Focus();
                    return;
                }
                if (reproductive_pressure < 1)
                {
                    MessageBox.Show("To nie jest poprawna liczba u³amkowa wiêksza ni¿ 1!");
                    reproductive_pressure = 2.0;
                    textBox14.Focus();
                    return;
                }
            }
            if (!string.IsNullOrWhiteSpace(textBox15.Text))
            {
                if (int.TryParse(textBox15.Text, out number_of_extension_of_the_child)) { }
                else
                {
                    MessageBox.Show("To nie jest poprawna liczba ca³kowita!");
                    textBox15.Focus();
                    return;
                }
            }

            if (checkBox1.Checked)
            {
                if (!string.IsNullOrWhiteSpace(textBox17.Text))
                {
                    if (double.TryParse(textBox17.Text, out tmp_mutation_probability)) { }
                    else
                    {
                        MessageBox.Show("To nie jest poprawna liczba ca³kowita!");
                        textBox17.Focus();
                        return;
                    }
                }
                else
                if (!string.IsNullOrWhiteSpace(textBox18.Text))
                {
                    if (int.TryParse(textBox18.Text, out downtime_length)) {
                        automatic_change_of_mutation_probability = true;    
                    }
                    else
                    {
                        MessageBox.Show("To nie jest poprawna liczba ca³kowita!");
                        textBox18.Focus();
                        return;
                    }
                }

            }
            else
                automatic_change_of_mutation_probability = false;

            if (generatedTests.Count == 0)
            {
                MessageBox.Show("Najpierw wygeneruj instancje.");
                return;
            }
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;


            int testsCount = generatedTests.Count;

            GAResult[] results = new GAResult[testsCount];

            Parallel.For(0, testsCount, test =>
            {
                results[test] =
                    RunGeneticAlgorithm(
                        generatedTests[test].Instance);
            });

            SaveCsv(
                generatedTests[0].M,
                generatedTests[0].N,
                generatedTests[0].ErrCount,
                generatedTests
                    .Select(x => x.GeneratorSolution)
                    .ToList(),
                results.ToList());

            MessageBox.Show("Test zakoñczony.");
            button1.Enabled = true;
            button2.Enabled = true;
            if (Instance.Count != 0)
                button3.Enabled = false;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (generatedTests.Count == 0)
            {
                MessageBox.Show("Brak wygenerowanych instancji.");
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "JSON files (*.json)|*.json";
            sfd.FileName = "instances.json";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string json = JsonSerializer.Serialize(
                    generatedTests,
                    new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });

                File.WriteAllText(sfd.FileName, json);

                MessageBox.Show(
                    $"Zapisano {generatedTests.Count} instancji.");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "JSON files (*.json)|*.json";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string json = File.ReadAllText(ofd.FileName);

                generatedTests =
                    JsonSerializer.Deserialize<List<TestInstance>>(json);

                if (generatedTests == null)
                {
                    generatedTests = new List<TestInstance>();

                    MessageBox.Show("Nie uda³o siê wczytaæ danych.");
                    return;
                }

                MessageBox.Show(
                    $"Wczytano {generatedTests.Count} instancji.");
            }
            button5.Enabled = true;
            button6.Enabled = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox17.Visible = true;
            textBox18.Visible = true;
            label26.Visible = true;
            label27.Visible = true;
        }

        /*GAResult[] results = new GAResult[testsCount];
        string[] opt_seqs = new string[testsCount];

        Parallel.For(0, testsCount, test =>
        {
            var generated = GenerateInstance(m, n, n_seq_opt, err_count);

            opt_seqs[test] = generated.SeqOpt;

            results[test] = RunGeneticAlgorithm(generated.Instance_local);
        });

        SaveCsv(m, n, err_count, opt_seqs.ToList(), results.ToList());

        MessageBox.Show("Test zakoñczony.");
    }
    */
    }
}
