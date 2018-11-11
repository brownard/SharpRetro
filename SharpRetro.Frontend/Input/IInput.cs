namespace SharpRetro.Frontend.Input
{
  public interface IInput
  {
    void OnInputPoll();
    short OnInputState(uint port, uint device, uint index, uint id);
  }
}