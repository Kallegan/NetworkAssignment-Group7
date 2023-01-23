using Alteruna;
using UnityEngine;

public class PlayerStateSync : Synchronizable
{

    public byte currentGameState;
    private byte _oldGameState;

    public override void AssembleData(Writer writer, byte LOD = 100)
    {
#if UNITY_EDITOR
        GameManager.Instance.PrintDebug("Assembling data", this.name);
#endif
        writer.Write(currentGameState);
    }

    public override void DisassembleData(Reader reader, byte LOD = 100)
    {
#if UNITY_EDITOR
        GameManager.Instance.PrintDebug("Disassembling data", this.name);
#endif
        currentGameState = reader.ReadByte();
        
        _oldGameState = currentGameState;
    }

    private void Update()
    {
        if (currentGameState != _oldGameState)
        {
            _oldGameState = currentGameState;
            
            Commit();
        }
        
        base.SyncUpdate();
    }

    public void SyncMyState()
    {
        Commit();
    }
}
