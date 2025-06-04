// In Models/CryptoCurrency.cs
using System;
using System.ComponentModel; // Required for INotifyPropertyChanged

namespace AccessibleCryptoViewer.Models // Ensure this namespace matches your project
{
    public class CryptoCurrency : INotifyPropertyChanged
    {
        // Backing fields for properties
        private string _id;
        private string _name;
        private string? _symbol; // << CHANGED TO NULLABLE STRING
        private decimal? _priceUsd;
        private double? _priceChangePercentage24h;
        private decimal? _volume24hUsd;
        private decimal? _marketCapUsd;
        private decimal? _circulatingSupply;
        private decimal? _totalSupply;
        private decimal? _liquidityUsd;
        private string _description;
        private string _homepageUrl;
        private string _blockchainExplorerUrl;
        private string _imageUrl;
        private string _chain;
        private string _contractAddress;
        private DateTime? _pairCreatedAt;
        private DateTime _lastUpdated;
        private string _dataSourceApi;

        public string Id
        {
            get => _id;
            set { if (_id != value) { _id = value; OnPropertyChanged(nameof(Id)); } }
        }

        public string Name
        {
            get => _name;
            set { if (_name != value) { _name = value; OnPropertyChanged(nameof(Name)); } }
        }

        public string? Symbol // << CHANGED TO NULLABLE STRING
        {
            get => _symbol;
            set { if (_symbol != value) { _symbol = value; OnPropertyChanged(nameof(Symbol)); } }
        }

        public decimal? PriceUsd
        {
            get => _priceUsd;
            set { if (_priceUsd != value) { _priceUsd = value; OnPropertyChanged(nameof(PriceUsd)); } }
        }

        public double? PriceChangePercentage24h
        {
            get => _priceChangePercentage24h;
            set { if (_priceChangePercentage24h != value) { _priceChangePercentage24h = value; OnPropertyChanged(nameof(PriceChangePercentage24h)); } }
        }

        public decimal? Volume24hUsd
        {
            get => _volume24hUsd;
            set { if (_volume24hUsd != value) { _volume24hUsd = value; OnPropertyChanged(nameof(Volume24hUsd)); } }
        }

        public decimal? MarketCapUsd
        {
            get => _marketCapUsd;
            set { if (_marketCapUsd != value) { _marketCapUsd = value; OnPropertyChanged(nameof(MarketCapUsd)); } }
        }

        public decimal? CirculatingSupply
        {
            get => _circulatingSupply;
            set { if (_circulatingSupply != value) { _circulatingSupply = value; OnPropertyChanged(nameof(CirculatingSupply)); } }
        }

        public decimal? TotalSupply
        {
            get => _totalSupply;
            set { if (_totalSupply != value) { _totalSupply = value; OnPropertyChanged(nameof(TotalSupply)); } }
        }

        public decimal? LiquidityUsd
        {
            get => _liquidityUsd;
            set { if (_liquidityUsd != value) { _liquidityUsd = value; OnPropertyChanged(nameof(LiquidityUsd)); } }
        }

        public string Description
        {
            get => _description;
            set { if (_description != value) { _description = value; OnPropertyChanged(nameof(Description)); } }
        }

        public string HomepageUrl
        {
            get => _homepageUrl;
            set { if (_homepageUrl != value) { _homepageUrl = value; OnPropertyChanged(nameof(HomepageUrl)); } }
        }

        public string BlockchainExplorerUrl
        {
            get => _blockchainExplorerUrl;
            set { if (_blockchainExplorerUrl != value) { _blockchainExplorerUrl = value; OnPropertyChanged(nameof(BlockchainExplorerUrl)); } }
        }

        public string ImageUrl
        {
            get => _imageUrl;
            set { if (_imageUrl != value) { _imageUrl = value; OnPropertyChanged(nameof(ImageUrl)); } }
        }

        public string Chain
        {
            get => _chain;
            set { if (_chain != value) { _chain = value; OnPropertyChanged(nameof(Chain)); } }
        }

        public string ContractAddress
        {
            get => _contractAddress;
            set { if (_contractAddress != value) { _contractAddress = value; OnPropertyChanged(nameof(ContractAddress)); } }
        }

        public DateTime? PairCreatedAt
        {
            get => _pairCreatedAt;
            set { if (_pairCreatedAt != value) { _pairCreatedAt = value; OnPropertyChanged(nameof(PairCreatedAt)); } }
        }

        public DateTime LastUpdated
        {
            get => _lastUpdated;
            set { if (_lastUpdated != value) { _lastUpdated = value; OnPropertyChanged(nameof(LastUpdated)); } }
        }

        public string DataSourceApi 
        {
            get => _dataSourceApi;
            set { if (_dataSourceApi != value) { _dataSourceApi = value; OnPropertyChanged(nameof(DataSourceApi)); } }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CryptoCurrency()
        {
            _id = string.Empty;
            _name = string.Empty;
            _symbol = null; // << CAN BE INITIALIZED TO NULL FOR NULLABLE STRING
            _description = string.Empty;
            _homepageUrl = string.Empty;
            _blockchainExplorerUrl = string.Empty;
            _imageUrl = string.Empty;
            _chain = string.Empty;
            _contractAddress = string.Empty;
            _dataSourceApi = string.Empty;
            LastUpdated = DateTime.UtcNow;
        }
    }
}