import { useState } from 'react';
import axiosInstance from '../api/axiosInstance';

export default function UpdateStockModal({ stock, onClose, onUpdated }) {
  const [form, setForm] = useState({
    targetPrice: stock.targetPrice,
    alertDirection: stock.alertDirection === 'Above' ? 0 : 1
  });
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    try {
      const response = await axiosInstance.put(`/watchlist/${stock.id}`, {
        targetPrice: parseFloat(form.targetPrice),
        alertDirection: parseInt(form.alertDirection)
      });
      onUpdated(response.data);
    } catch (err) {
      setError(err.response?.data?.message || 'Güncelleme başarısız.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="fixed inset-0 bg-black bg-opacity-40 flex items-center justify-center z-50">
      <div className="bg-white rounded-xl shadow-xl w-full max-w-md p-6">

        <div className="flex items-center justify-between mb-4">
          <h2 className="text-lg font-bold text-gray-800">
            {stock.symbol} Güncelle
          </h2>
          <button onClick={onClose} className="text-gray-400 hover:text-gray-600 text-xl">
            ✕
          </button>
        </div>

        {/* Mevcut fiyat bilgisi */}
        <div className="bg-gray-50 rounded-lg px-4 py-3 mb-4 text-sm text-gray-600">
          Anlık fiyat: <span className="font-bold">${stock.currentPrice.toFixed(2)}</span>
        </div>

        {error && (
          <div className="bg-red-100 text-red-600 px-3 py-2 rounded mb-3 text-sm">
            {error}
          </div>
        )}

        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Yeni Hedef Fiyat ($)
            </label>
            <input
              type="number"
              name="targetPrice"
              value={form.targetPrice}
              onChange={handleChange}
              required
              min="0.01"
              step="0.01"
              className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Bildirim Yönü
            </label>
            <div className="flex gap-3">
              <label className="flex items-center gap-2 cursor-pointer">
                <input
                  type="radio"
                  name="alertDirection"
                  value={0}
                  checked={parseInt(form.alertDirection) === 0}
                  onChange={handleChange}
                />
                <span className="text-sm">↑ Üstüne çıkınca</span>
              </label>
              <label className="flex items-center gap-2 cursor-pointer">
                <input
                  type="radio"
                  name="alertDirection"
                  value={1}
                  checked={parseInt(form.alertDirection) === 1}
                  onChange={handleChange}
                />
                <span className="text-sm">↓ Altına düşünce</span>
              </label>
            </div>
          </div>

          <div className="flex gap-3 pt-2">
            <button
              type="button"
              onClick={onClose}
              className="flex-1 bg-gray-100 text-gray-600 py-2 rounded-lg hover:bg-gray-200 transition"
            >
              İptal
            </button>
            <button
              type="submit"
              disabled={loading}
              className="flex-1 bg-blue-600 text-white py-2 rounded-lg hover:bg-blue-700 transition disabled:opacity-50"
            >
              {loading ? 'Kaydediliyor...' : 'Kaydet'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}