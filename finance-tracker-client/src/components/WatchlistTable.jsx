import { useState } from 'react';
import axiosInstance from '../api/axiosInstance';
import UpdateStockModal from './UpdateStockModal';

export default function WatchlistTable({ watchlist, onRefresh, onDelete, setWatchlist }) {
  const [updatingId, setUpdatingId] = useState(null);
  const [updateStock, setUpdateStock] = useState(null);

  const handleRefreshClick = async (id) => {
    setUpdatingId(id);
    await onRefresh(id);
    setUpdatingId(null);
  };

  const handleUpdated = (updatedStock) => {
    setWatchlist(prev =>
      prev.map(item => item.id === updatedStock.id ? updatedStock : item)
    );
    setUpdateStock(null);
  };


  return (
    <>
      <div className="bg-white rounded-xl shadow-sm overflow-hidden">
        <table className="w-full">
          <thead>
            <tr className="bg-gray-50 border-b border-gray-200">
              <th className="text-left px-6 py-3 text-sm font-semibold text-gray-600">Hisse</th>
              <th className="text-right px-6 py-3 text-sm font-semibold text-gray-600">Anlık Fiyat</th>
              <th className="text-right px-6 py-3 text-sm font-semibold text-gray-600">Hedef</th>
              <th className="text-center px-6 py-3 text-sm font-semibold text-gray-600">Yön</th>
              <th className="text-center px-6 py-3 text-sm font-semibold text-gray-600">Durum</th>
              <th className="text-center px-6 py-3 text-sm font-semibold text-gray-600">İşlem</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-gray-100">
            {watchlist.map(item => (
              <WatchlistRow
                key={item.id}
                item={item}
                onRefresh={() => handleRefreshClick(item.id)}
                onDelete={() => onDelete(item.id)}
                onEdit={() => setUpdateStock(item)}
                isUpdating={updatingId === item.id}
              />
            ))}
          </tbody>
        </table>
      </div>

      {/* Güncelleme modalı */}
      {updateStock && (
        <UpdateStockModal
          stock={updateStock}
          onClose={() => setUpdateStock(null)}
          onUpdated={handleUpdated}
        />
      )}
    </>
  );
}

function WatchlistRow({ item, onRefresh, onDelete, onEdit, isUpdating }) {
  const isAbove = item.alertDirection === 'Above';
  const targetReached = item.alertTriggered;

  return (
    <tr className="hover:bg-gray-50 transition">
      {/* Hisse sembolü */}
      <td className="px-6 py-4">
        <div className="font-bold text-gray-800">{item.symbol}</div>
        <div className="text-xs text-gray-400">{item.companyName || '—'}</div>
      </td>

      {/* Anlık fiyat */}
      <td className="px-6 py-4 text-right">
        <span className="font-mono font-semibold text-gray-800">
          ${item.currentPrice.toFixed(2)}
        </span>
      </td>

      {/* Hedef fiyat */}
      <td className="px-6 py-4 text-right">
        <span className="font-mono text-gray-600">
          ${item.targetPrice.toFixed(2)}
        </span>
      </td>

      {/* Yön */}
      <td className="px-6 py-4 text-center">
        <span className={`text-lg ${isAbove ? 'text-green-500' : 'text-red-500'}`}>
          {isAbove ? '↑' : '↓'}
        </span>
      </td>

      {/* Durum */}
      <td className="px-6 py-4 text-center">
        {targetReached ? (
          <span className="bg-green-100 text-green-700 text-xs px-2 py-1 rounded-full font-medium">
            🎯 Hedefe Ulaştı
          </span>
        ) : (
          <span className="bg-gray-100 text-gray-500 text-xs px-2 py-1 rounded-full">
            Bekliyor
          </span>
        )}
      </td>

      {/* İşlem butonları */}
      <td className="px-6 py-4">
        <div className="flex items-center justify-center gap-2">
          {/* Güncelle */}
          <button
            onClick={onEdit}
            className="p-1.5 text-blue-500 hover:bg-blue-50 rounded-lg transition"
            title="Güncelle"
          >
            ✏️
          </button>

          {/* Yenile */}
          <button
            onClick={onRefresh}
            disabled={isUpdating}
            className="p-1.5 text-green-500 hover:bg-green-50 rounded-lg transition disabled:opacity-50"
            title="Fiyatı Yenile"
          >
            {isUpdating ? '⏳' : '🔄'}
          </button>

          {/* Sil */}
          <button
            onClick={onDelete}
            className="p-1.5 text-red-500 hover:bg-red-50 rounded-lg transition"
            title="Sil"
          >
            🗑️
          </button>
        </div>
      </td>
    </tr>
  );
}