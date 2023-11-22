using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Terra
{
    public const int MOVE_SLOTS = 4;
    private const float BASE_STAT_MULTIPLIER = 0.033f;

    [SerializeField] private TerraBase terraBase;
    [SerializeField] private string terraNickname;
    [SerializeField] [Range(1, 100)] private int level;
    [SerializeField] private List<TerraMove> moves;
    [SerializeField] private StatusEffect statusEffect;
    [SerializeField] [Min(0)] private int currentHP;

    public Terra(TerraBase terraBase)
    {
        this.terraBase = terraBase;
        terraNickname = terraBase.GetSpeciesName();
        level = 1;
        moves = new List<TerraMove>();
        GenerateNaturalMoveSet();
        statusEffect = null;
        currentHP = GetMaxHP();
    }

    public Terra(TerraBase terraBase, int level)
    {
        this.terraBase = terraBase;
        terraNickname = terraBase.GetSpeciesName();
        this.level = level;
        moves = new List<TerraMove>();
        GenerateNaturalMoveSet();
        statusEffect = null;
        currentHP = GetMaxHP();
    }

    public Terra(TerraSavable terraSavable)
    {
        terraBase = SODatabase.GetInstance().GetTerraByName(terraSavable.GetTerraBaseName());
        terraNickname = terraSavable.GetTerraNickname();
        level = terraSavable.GetLevel();
        moves = new List<TerraMove>();
        for(int i = 0; i < terraSavable.GetSavableMoves().Count; i++)
            moves.Add(new TerraMove(terraSavable.GetSavableMoves()[i]));
        statusEffect = terraSavable.GetStatusEffect();
        currentHP = terraSavable.GetCurrentHP();
    }

    public void GenerateNaturalMoveSet()
    {
        moves.Clear();

        for(int i = level; i > 0; i--) {
            foreach(LearnedMove learnedMove in terraBase.GetNaturalMovePool()) {
                if(learnedMove.GetLevelLearned() == i) {
                    moves.Insert(0, learnedMove.GetMove());

                    if (moves.Count >= MOVE_SLOTS)
                        return;
                }
            }
        }
    }

    public bool AddMove(TerraMove move)
    {
        if (moves.Count >= MOVE_SLOTS)
            return false;

        moves.Add(move);
        return true;
    }

    public bool SetMoveInSlot(TerraMove move, int slotIndex)
    {
        if (slotIndex >= MOVE_SLOTS)
            return false;

        moves[slotIndex] = move;
        return true;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP < 0)
            currentHP = 0;
        if (currentHP > GetMaxHP())
            currentHP = GetMaxHP();
    }

    public override string ToString()
    {
        return terraBase.GetSpeciesName();
    }

    public TerraBase GetTerraBase() { return terraBase; }

    public void SetTerraBase(TerraBase terraBase) { this.terraBase = terraBase; }

    public string GetTerraNickname() {  return terraNickname; }

    public void SetTerraNickname(string terraNickname) {  this.terraNickname = terraNickname; }

    public int GetLevel() { return level; }

    public void SetLevel(int level) { this.level = level; }

    public List<TerraMove> GetMoves() { return moves; }

    public void SetMoveSet(List<TerraMove> newMoves)
    {
        moves.Clear();

        for(int i = 0; i < MOVE_SLOTS; i++)
            moves.Add(newMoves[i]);
    }

    public int GetCurrentHP() { return currentHP; }

    public void SetCurrentHP(int currentHP)
    {
        this.currentHP = (currentHP >= 0) ? currentHP : 0;
        if (currentHP > GetMaxHP())
            this.currentHP = GetMaxHP();
    }

    public StatusEffect GetStatusEffect() { return statusEffect; }

    public void SetStatusEffect(StatusEffect statusEffect) { this.statusEffect = statusEffect; }

    public int GetMaxHP() { return Mathf.FloorToInt(level * terraBase.GetBaseHP() * BASE_STAT_MULTIPLIER + 10); }

    public int GetAttack() { return Mathf.FloorToInt(level * terraBase.GetBaseAttack() * BASE_STAT_MULTIPLIER + 5); }

    public int GetDefence() { return Mathf.FloorToInt(level * terraBase.GetBaseDefence() * BASE_STAT_MULTIPLIER + 5); }

    public int GetSpAttack() { return Mathf.FloorToInt(level * terraBase.GetBaseSpAttack() * BASE_STAT_MULTIPLIER + 5); }

    public int GetSpDefence() { return Mathf.FloorToInt(level * terraBase.GetBaseSpDefence() * BASE_STAT_MULTIPLIER + 5); }

    public int GetSpeed() { return Mathf.FloorToInt(level * terraBase.GetBaseSpeed() * BASE_STAT_MULTIPLIER + 5); }
}