import { useState } from 'react';
import axiosInstance from '../api/axiosInstance';

export default function AddStockModal({ onClose, onAdded }) {
  const [form, setForm] = useState({
    symbol: '',
    targetPrice: '',
    alertDirection: 0
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
      const response = await axiosInstance.post('/watchlist', {
        symbol: form.symbol.toUpperCase(),
        targetPrice: parseFloat(form.targetPrice),
        alertDirection: parseInt(form.alertDirection)
      });
      onAdded(response.data);
    } catch (err) {
      setError(err.response?.data?.message || 'Hisse eklenemedi.');
    } finally {
      setLoading(false);
    }
  };

  return (
    // Arka plan overlay
    <div className="fixed inset-0 bg-black bg-opacity-40 flex items-center justify-center z-50">
      <div className="bg-white rounded-xl shadow-xl w-full max-w-md p-6">

        {/* Başlık */}
        <div className="flex items-center justify-between mb-4">
          <h2 className="text-lg font-bold text-gray-800">Hisse Ekle</h2>
          <button
            onClick={onClose}
            className="text-gray-400 hover:text-gray-600 text-xl"
          >
            ✕
          </button>
        </div>

        {/* Hata */}
        {error && (
          <div className="bg-red-100 text-red-600 px-3 py-2 rounded mb-3 text-sm">
            {error}
          </div>
        )}

        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Hisse Sembolü
            </label>
            <input
              type="text"
              name="symbol"
              value={form.symbol}
              onChange={handleChange}
              required
              maxLength={10}
              className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500 uppercase"
              placeholder="AAPL, TSLA, MSFT..."
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Hedef Fiyat ($)
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
              placeholder="200.00"
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
                  className="text-blue-600"
                />
                <span className="text-sm text-gray-700">
                  ↑ Üstüne çıkınca bildir
                </span>
              </label>
              <label className="flex items-center gap-2 cursor-pointer">
                <input
                  type="radio"
                  name="alertDirection"
                  value={1}
                  checked={parseInt(form.alertDirection) === 1}
                  onChange={handleChange}
                  className="text-blue-600"
                />
                <span className="text-sm text-gray-700">
                  ↓ Altına düşünce bildir
                </span>
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
              {loading ? 'Ekleniyor...' : 'Ekle'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}