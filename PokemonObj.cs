using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonObj : MonoBehaviour
{
    // Enumérations définissant les types de Pokémon
    public enum TypePokemon {
        Normal,
        Fire, 
        Water, 
        Grass, 
        Electric,
        Ice,
        Fighting,
        Poison, 
        Ground, 
        Flying, 
        Psychic, 
        Bug, 
        Rock, 
        Ghost, 
        Dark, 
        Dragon, 
        Steel, 
        Fairy 
    };

    [Serializable]
    // Structure représentant les données d'un Pokémon
    public struct PokemonData
    {

        [SerializeField] private string Name; // Nom du Pokémon
        [SerializeField] private int HP; // Points de vie du Pokémon
        [SerializeField] private int Atk; // Attaque du Pokémon
        [SerializeField] private int Def; // Défense du Pokémon
        [SerializeField] private float Weight; // Poids du Pokémon
        [SerializeField] private TypePokemon Type; // Type du Pokémon
        [SerializeField] private TypePokemon[] Pokemon_weaknesses; // Faiblesses du Pokémon
        [SerializeField] private TypePokemon[] Pokemon_resistances; // Résistances du Pokémon


        private int currentHp; // Points de vie actuels du Pokémon
        private int stats; // Statistiques du Pokémon
        private int statsPoints; // Points de statistiques du Pokémon

        // Propriétés pour accéder aux champs
        public string GetName { get { return Name; } }
        public int GetHP { get { return HP; } }
        public int GetAtk { get { return Atk; } }
        public int GetDef { get { return Def; } }
        public float GetWeight { get { return Weight; } }
        public TypePokemon GetTypePokemon { get { return Type; } }
        public TypePokemon[] GetWeaknesses { get { return Pokemon_weaknesses; } }
        public TypePokemon[] GetResistances { get { return Pokemon_resistances; } }
        public int GetCurrentHp { get { return currentHp; } set { currentHp = value; } }
        public int GetStats { get { return stats; } }
        public int GetStatsPoints { get { return statsPoints; } set { statsPoints = value; } }


        public PokemonData(string Name, int HP, int Atk, int Def, float Weight, TypePokemon Type, TypePokemon[] Pokemon_weaknesses, TypePokemon[] Pokemon_resistances, int currentHp, int stats, int statsPoints)
        {
            this.Name = Name;
            this.HP = HP;
            this.Atk = Atk;
            this.Def = Def;
            this.Weight = Weight;
            this.Type = Type;
            this.Pokemon_weaknesses = Pokemon_weaknesses;
            this.Pokemon_resistances = Pokemon_resistances;
            this.currentHp = currentHp;
            this.stats = stats;
            this.statsPoints = statsPoints;
        }
    }

    // Structure représentant les données d'un Pokémon
    public PokemonData pokemonData; 

    // Référence au Pokémon adversaire
    [SerializeField] private PokemonOpponent PokemonOpponent;

    // Méthode pour initialiser les points de vie actuels d'un Pokémon
    public void InitCurrentLife(PokemonData pokemonData)
    {
        pokemonData.GetCurrentHp = pokemonData.GetHP;
    }

    // Méthode pour initialiser les points de statistiques d'un Pokémon
    public void InitStatsPoints(PokemonData pokemonData)
    {
        pokemonData.GetStatsPoints = pokemonData.GetHP + pokemonData.GetAtk + pokemonData.GetDef;
    }

    // Méthode pour vérifier si un Pokémon est en vie
    public bool IsPokemonAlive(PokemonData pokemonData)
    {
        return pokemonData.GetCurrentHp > 0;
    }

    // Méthode pour obtenir les dégâts d'attaque d'un Pokémon
    public int GetAttackDamage(PokemonData pokemonData)
    {
        return pokemonData.GetAtk;
    }

    // Méthode pour infliger des dégâts à un Pokémon
    public void TakeDamage(PokemonData pokemonData, int damage)
    {
        // Vérifier les faiblesses et appliquer des dégâts doubles
        for (int i = 0; i < pokemonData.GetWeaknesses.Length; i++)
        {
            if (pokemonData.GetWeaknesses[i] == PokemonOpponent.pokemonData.GetTypePokemon)
            {
                pokemonData.GetCurrentHp -= damage * 2;
            }
        }

        // Vérifier les résistances et appliquer des dégâts réduits
        for (int i = 0; i < pokemonData.GetResistances.Length; i++)
        {
            if (pokemonData.GetResistances[i] == PokemonOpponent.pokemonData.GetTypePokemon)
            {
                pokemonData.GetCurrentHp -= damage / 2;
            }
        }

        // Appliquer les dégâts normaux
        pokemonData.GetCurrentHp -= damage;
    }

    // Méthode pour faire attaquer le Pokémon adversaire
    public void AttackOpponent(PokemonData pokemonData)
    {
        if (IsPokemonAlive(pokemonData))
        {
            PokemonOpponent.TakeDamage(PokemonOpponent.pokemonData, GetAttackDamage(pokemonData));
        }
        else
        {
            Debug.Log("Le pokémon ne peut plus se battre.");
        }
    }

    // Méthode Start pour lancer l'attaque sur le Pokémon adversaire de manière aléatoire
    private void Start()
    {
        StartCoroutine(AttackOpponentRandomly());
    }

    // Coroutine pour attaquer le Pokémon adversaire de manière aléatoire
    private IEnumerator AttackOpponentRandomly()
    {
        while (true)
        {
            float delay = UnityEngine.Random.Range(1.25f, 3.1f);
            yield return new WaitForSeconds(delay);
            AttackOpponent(pokemonData);
        }
    }
}
