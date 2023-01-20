using Alteruna;
using UnityEngine;

public class PlayerGameStateSync : Synchronizable
{

    public byte currentGameState;
    private byte _oldGameState;
    
    public override void AssembleData(Writer writer, byte LOD = 100)
    {
        GameManager.Instance.PrintDebug("Assembling data", this.name);
        writer.Write(currentGameState);
    }

    public override void DisassembleData(Reader reader, byte LOD = 100)
    {
        GameManager.Instance.PrintDebug("Disassembling data", this.name);

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
}
