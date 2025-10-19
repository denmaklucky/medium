using System.ComponentModel.DataAnnotations;

namespace CSharpEfAndAspNet;

public sealed record CreateNoteRequest([Required] [MaxLength(256)] string Value);