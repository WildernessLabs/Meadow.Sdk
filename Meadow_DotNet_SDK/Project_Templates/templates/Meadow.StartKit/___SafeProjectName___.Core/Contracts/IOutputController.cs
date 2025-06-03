using System;
using System.Threading.Tasks;

namespace ___SafeProjectName___.Core;

public interface IOutputController
{
    Task SetState(bool state);
}