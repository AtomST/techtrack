// src/components/common/StatusBadge.tsx
export function StatusBadge({ status }: { status: string }) {
  const color =
    status === "Green"
      ? "bg-green-500"
      : status === "Yellow"
      ? "bg-yellow-500"
      : "bg-red-500";

  return (
    <span className={`text-xs px-2 text-white rounded ${color}`}>
      {status}
    </span>
  );
}
