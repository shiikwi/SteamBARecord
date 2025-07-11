// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace FlatData
{

using global::System;
using global::System.Collections.Generic;
using global::Google.FlatBuffers;

public struct AcademyFavorScheduleExcel : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_25_2_10(); }
  public static AcademyFavorScheduleExcel GetRootAsAcademyFavorScheduleExcel(ByteBuffer _bb) { return GetRootAsAcademyFavorScheduleExcel(_bb, new AcademyFavorScheduleExcel()); }
  public static AcademyFavorScheduleExcel GetRootAsAcademyFavorScheduleExcel(ByteBuffer _bb, AcademyFavorScheduleExcel obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public AcademyFavorScheduleExcel __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public long Id { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetLong(o + __p.bb_pos) : (long)0; } }
  public long CharacterId { get { int o = __p.__offset(6); return o != 0 ? __p.bb.GetLong(o + __p.bb_pos) : (long)0; } }
  public long ScheduleGroupId { get { int o = __p.__offset(8); return o != 0 ? __p.bb.GetLong(o + __p.bb_pos) : (long)0; } }
  public long OrderInGroup { get { int o = __p.__offset(10); return o != 0 ? __p.bb.GetLong(o + __p.bb_pos) : (long)0; } }
  public string Location { get { int o = __p.__offset(12); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetLocationBytes() { return __p.__vector_as_span<byte>(12, 1); }
#else
  public ArraySegment<byte>? GetLocationBytes() { return __p.__vector_as_arraysegment(12); }
#endif
  public byte[] GetLocationArray() { return __p.__vector_as_array<byte>(12); }
  public uint LocalizeScenarioId { get { int o = __p.__offset(14); return o != 0 ? __p.bb.GetUint(o + __p.bb_pos) : (uint)0; } }
  public long FavorRank { get { int o = __p.__offset(16); return o != 0 ? __p.bb.GetLong(o + __p.bb_pos) : (long)0; } }
  public long SecretStoneAmount { get { int o = __p.__offset(18); return o != 0 ? __p.bb.GetLong(o + __p.bb_pos) : (long)0; } }
  public long ScenarioSriptGroupId { get { int o = __p.__offset(20); return o != 0 ? __p.bb.GetLong(o + __p.bb_pos) : (long)0; } }
  public FlatData.ParcelType RewardParcelType(int j) { int o = __p.__offset(22); return o != 0 ? (FlatData.ParcelType)__p.bb.GetInt(__p.__vector(o) + j * 4) : (FlatData.ParcelType)0; }
  public int RewardParcelTypeLength { get { int o = __p.__offset(22); return o != 0 ? __p.__vector_len(o) : 0; } }
#if ENABLE_SPAN_T
  public Span<FlatData.ParcelType> GetRewardParcelTypeBytes() { return __p.__vector_as_span<FlatData.ParcelType>(22, 4); }
#else
  public ArraySegment<byte>? GetRewardParcelTypeBytes() { return __p.__vector_as_arraysegment(22); }
#endif
  public FlatData.ParcelType[] GetRewardParcelTypeArray() { int o = __p.__offset(22); if (o == 0) return null; int p = __p.__vector(o); int l = __p.__vector_len(o); FlatData.ParcelType[] a = new FlatData.ParcelType[l]; for (int i = 0; i < l; i++) { a[i] = (FlatData.ParcelType)__p.bb.GetInt(p + i * 4); } return a; }
  public long RewardParcelId(int j) { int o = __p.__offset(24); return o != 0 ? __p.bb.GetLong(__p.__vector(o) + j * 8) : (long)0; }
  public int RewardParcelIdLength { get { int o = __p.__offset(24); return o != 0 ? __p.__vector_len(o) : 0; } }
#if ENABLE_SPAN_T
  public Span<long> GetRewardParcelIdBytes() { return __p.__vector_as_span<long>(24, 8); }
#else
  public ArraySegment<byte>? GetRewardParcelIdBytes() { return __p.__vector_as_arraysegment(24); }
#endif
  public long[] GetRewardParcelIdArray() { return __p.__vector_as_array<long>(24); }
  public long RewardAmount(int j) { int o = __p.__offset(26); return o != 0 ? __p.bb.GetLong(__p.__vector(o) + j * 8) : (long)0; }
  public int RewardAmountLength { get { int o = __p.__offset(26); return o != 0 ? __p.__vector_len(o) : 0; } }
#if ENABLE_SPAN_T
  public Span<long> GetRewardAmountBytes() { return __p.__vector_as_span<long>(26, 8); }
#else
  public ArraySegment<byte>? GetRewardAmountBytes() { return __p.__vector_as_arraysegment(26); }
#endif
  public long[] GetRewardAmountArray() { return __p.__vector_as_array<long>(26); }

  public static Offset<FlatData.AcademyFavorScheduleExcel> CreateAcademyFavorScheduleExcel(FlatBufferBuilder builder,
      long id = 0,
      long character_id = 0,
      long schedule_group_id = 0,
      long order_in_group = 0,
      StringOffset locationOffset = default(StringOffset),
      uint localize_scenario_id = 0,
      long favor_rank = 0,
      long secret_stone_amount = 0,
      long scenario_sript_group_id = 0,
      VectorOffset reward_parcel_typeOffset = default(VectorOffset),
      VectorOffset reward_parcel_idOffset = default(VectorOffset),
      VectorOffset reward_amountOffset = default(VectorOffset)) {
    builder.StartTable(12);
    AcademyFavorScheduleExcel.AddScenarioSriptGroupId(builder, scenario_sript_group_id);
    AcademyFavorScheduleExcel.AddSecretStoneAmount(builder, secret_stone_amount);
    AcademyFavorScheduleExcel.AddFavorRank(builder, favor_rank);
    AcademyFavorScheduleExcel.AddOrderInGroup(builder, order_in_group);
    AcademyFavorScheduleExcel.AddScheduleGroupId(builder, schedule_group_id);
    AcademyFavorScheduleExcel.AddCharacterId(builder, character_id);
    AcademyFavorScheduleExcel.AddId(builder, id);
    AcademyFavorScheduleExcel.AddRewardAmount(builder, reward_amountOffset);
    AcademyFavorScheduleExcel.AddRewardParcelId(builder, reward_parcel_idOffset);
    AcademyFavorScheduleExcel.AddRewardParcelType(builder, reward_parcel_typeOffset);
    AcademyFavorScheduleExcel.AddLocalizeScenarioId(builder, localize_scenario_id);
    AcademyFavorScheduleExcel.AddLocation(builder, locationOffset);
    return AcademyFavorScheduleExcel.EndAcademyFavorScheduleExcel(builder);
  }

  public static void StartAcademyFavorScheduleExcel(FlatBufferBuilder builder) { builder.StartTable(12); }
  public static void AddId(FlatBufferBuilder builder, long id) { builder.AddLong(0, id, 0); }
  public static void AddCharacterId(FlatBufferBuilder builder, long characterId) { builder.AddLong(1, characterId, 0); }
  public static void AddScheduleGroupId(FlatBufferBuilder builder, long scheduleGroupId) { builder.AddLong(2, scheduleGroupId, 0); }
  public static void AddOrderInGroup(FlatBufferBuilder builder, long orderInGroup) { builder.AddLong(3, orderInGroup, 0); }
  public static void AddLocation(FlatBufferBuilder builder, StringOffset locationOffset) { builder.AddOffset(4, locationOffset.Value, 0); }
  public static void AddLocalizeScenarioId(FlatBufferBuilder builder, uint localizeScenarioId) { builder.AddUint(5, localizeScenarioId, 0); }
  public static void AddFavorRank(FlatBufferBuilder builder, long favorRank) { builder.AddLong(6, favorRank, 0); }
  public static void AddSecretStoneAmount(FlatBufferBuilder builder, long secretStoneAmount) { builder.AddLong(7, secretStoneAmount, 0); }
  public static void AddScenarioSriptGroupId(FlatBufferBuilder builder, long scenarioSriptGroupId) { builder.AddLong(8, scenarioSriptGroupId, 0); }
  public static void AddRewardParcelType(FlatBufferBuilder builder, VectorOffset rewardParcelTypeOffset) { builder.AddOffset(9, rewardParcelTypeOffset.Value, 0); }
  public static VectorOffset CreateRewardParcelTypeVector(FlatBufferBuilder builder, FlatData.ParcelType[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddInt((int)data[i]); return builder.EndVector(); }
  public static VectorOffset CreateRewardParcelTypeVectorBlock(FlatBufferBuilder builder, FlatData.ParcelType[] data) { builder.StartVector(4, data.Length, 4); builder.Add(data); return builder.EndVector(); }
  public static VectorOffset CreateRewardParcelTypeVectorBlock(FlatBufferBuilder builder, ArraySegment<FlatData.ParcelType> data) { builder.StartVector(4, data.Count, 4); builder.Add(data); return builder.EndVector(); }
  public static VectorOffset CreateRewardParcelTypeVectorBlock(FlatBufferBuilder builder, IntPtr dataPtr, int sizeInBytes) { builder.StartVector(1, sizeInBytes, 1); builder.Add<FlatData.ParcelType>(dataPtr, sizeInBytes); return builder.EndVector(); }
  public static void StartRewardParcelTypeVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static void AddRewardParcelId(FlatBufferBuilder builder, VectorOffset rewardParcelIdOffset) { builder.AddOffset(10, rewardParcelIdOffset.Value, 0); }
  public static VectorOffset CreateRewardParcelIdVector(FlatBufferBuilder builder, long[] data) { builder.StartVector(8, data.Length, 8); for (int i = data.Length - 1; i >= 0; i--) builder.AddLong(data[i]); return builder.EndVector(); }
  public static VectorOffset CreateRewardParcelIdVectorBlock(FlatBufferBuilder builder, long[] data) { builder.StartVector(8, data.Length, 8); builder.Add(data); return builder.EndVector(); }
  public static VectorOffset CreateRewardParcelIdVectorBlock(FlatBufferBuilder builder, ArraySegment<long> data) { builder.StartVector(8, data.Count, 8); builder.Add(data); return builder.EndVector(); }
  public static VectorOffset CreateRewardParcelIdVectorBlock(FlatBufferBuilder builder, IntPtr dataPtr, int sizeInBytes) { builder.StartVector(1, sizeInBytes, 1); builder.Add<long>(dataPtr, sizeInBytes); return builder.EndVector(); }
  public static void StartRewardParcelIdVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(8, numElems, 8); }
  public static void AddRewardAmount(FlatBufferBuilder builder, VectorOffset rewardAmountOffset) { builder.AddOffset(11, rewardAmountOffset.Value, 0); }
  public static VectorOffset CreateRewardAmountVector(FlatBufferBuilder builder, long[] data) { builder.StartVector(8, data.Length, 8); for (int i = data.Length - 1; i >= 0; i--) builder.AddLong(data[i]); return builder.EndVector(); }
  public static VectorOffset CreateRewardAmountVectorBlock(FlatBufferBuilder builder, long[] data) { builder.StartVector(8, data.Length, 8); builder.Add(data); return builder.EndVector(); }
  public static VectorOffset CreateRewardAmountVectorBlock(FlatBufferBuilder builder, ArraySegment<long> data) { builder.StartVector(8, data.Count, 8); builder.Add(data); return builder.EndVector(); }
  public static VectorOffset CreateRewardAmountVectorBlock(FlatBufferBuilder builder, IntPtr dataPtr, int sizeInBytes) { builder.StartVector(1, sizeInBytes, 1); builder.Add<long>(dataPtr, sizeInBytes); return builder.EndVector(); }
  public static void StartRewardAmountVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(8, numElems, 8); }
  public static Offset<FlatData.AcademyFavorScheduleExcel> EndAcademyFavorScheduleExcel(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<FlatData.AcademyFavorScheduleExcel>(o);
  }
}


static public class AcademyFavorScheduleExcelVerify
{
  static public bool Verify(Google.FlatBuffers.Verifier verifier, uint tablePos)
  {
    return verifier.VerifyTableStart(tablePos)
      && verifier.VerifyField(tablePos, 4 /*Id*/, 8 /*long*/, 8, false)
      && verifier.VerifyField(tablePos, 6 /*CharacterId*/, 8 /*long*/, 8, false)
      && verifier.VerifyField(tablePos, 8 /*ScheduleGroupId*/, 8 /*long*/, 8, false)
      && verifier.VerifyField(tablePos, 10 /*OrderInGroup*/, 8 /*long*/, 8, false)
      && verifier.VerifyString(tablePos, 12 /*Location*/, false)
      && verifier.VerifyField(tablePos, 14 /*LocalizeScenarioId*/, 4 /*uint*/, 4, false)
      && verifier.VerifyField(tablePos, 16 /*FavorRank*/, 8 /*long*/, 8, false)
      && verifier.VerifyField(tablePos, 18 /*SecretStoneAmount*/, 8 /*long*/, 8, false)
      && verifier.VerifyField(tablePos, 20 /*ScenarioSriptGroupId*/, 8 /*long*/, 8, false)
      && verifier.VerifyVectorOfData(tablePos, 22 /*RewardParcelType*/, 4 /*FlatData.ParcelType*/, false)
      && verifier.VerifyVectorOfData(tablePos, 24 /*RewardParcelId*/, 8 /*long*/, false)
      && verifier.VerifyVectorOfData(tablePos, 26 /*RewardAmount*/, 8 /*long*/, false)
      && verifier.VerifyTableEnd(tablePos);
  }
}

}
