using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thirdweb;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class BlockchainManager : MonoBehaviour
{
    public string Address { get; private set; }

    public Button nftButton;
    public Button playButton;
    public Button shopButton;

    //Game Shop
    public Button freeNFTButton;
    public Button coin500Button;
    public Button coin5000Button;
    public Button coin10000Button;
    public Button backButton;

    public TextMeshProUGUI coinBoughtText;
    public TextMeshProUGUI buyingStatusText;
    public TextMeshProUGUI claimNFTStatusText;

    public GameObject gameShopPanel;

    string NFTAddressSmartContract = "0x109c9d88aa8B43e1ea02ccBe3a3FF8fE2d14DFAB";
    //Thay bằng NFT mới khi Deploy thành công
    string freeNFTAddressSmartContract = "0x109c9d88aa8B43e1ea02ccBe3a3FF8fE2d14DFAB";

    private string receiverAddress = "0xb5A4FB8F5aFC725113bEE5c9Fc99f52059D6256F";


    private void Start()
    {
        nftButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);
        shopButton.gameObject.SetActive(false);
        claimNFTStatusText.gameObject.SetActive(false);
        gameShopPanel.SetActive(false);
    }

    public async void Login()
    {
        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(NFTAddressSmartContract);
        List<NFT> nftList = await contract.ERC721.GetOwned(Address);
        if (nftList.Count == 0)
        {
            nftButton.gameObject.SetActive(true);
        }
        else
        {
            playButton.gameObject.SetActive(true);
            shopButton.gameObject.SetActive(true);
        }
    }

    public async void ClaimNFTPass()
    {
        claimNFTStatusText.text = "Claiming...";
        claimNFTStatusText.gameObject.SetActive(true);
        nftButton.interactable = false;
        var contract = ThirdwebManager.Instance.SDK.GetContract(NFTAddressSmartContract);
        try
        {
            var result = await contract.ERC721.ClaimTo(Address, 1);
            claimNFTStatusText.text = "Claimed NFT Pass!";
            nftButton.gameObject.SetActive(false);
            playButton.gameObject.SetActive(true);
            shopButton.gameObject.SetActive(true);
        }
        catch (Exception ex)
        {
            claimNFTStatusText.text = $"Failed to claim NFT: {ex.Message}";
            Debug.LogError("Error while claiming NFT: " + ex);
            nftButton.gameObject.SetActive(true);
            playButton.gameObject.SetActive(false);
            shopButton.gameObject.SetActive(false);
        }

    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Logo");
    }

    public void OpenGameShopPanel() {
        gameShopPanel.gameObject.SetActive (true);
    }
    public void CloseGameShopPanel()
    {
        gameShopPanel.gameObject.SetActive(false);
    }


    private static int ConvertStringToRoundedInt(string numberStr)
    {
        // Convert the string to a double
        double number = double.Parse(numberStr);

        // Round the number
        double roundedNumber = Math.Round(number);

        // Convert to int and return
        return (int)roundedNumber;
    }

    public async void SpendTokenToBuyCoins(int indexValue)
    {

        freeNFTButton.interactable = false;
        coin500Button.interactable = false;
        coin5000Button.interactable = false;
        coin10000Button.interactable = false;
        backButton.interactable = false;

        string costValue = "0";
        buyingStatusText.text = "Buying...";
        buyingStatusText.gameObject.SetActive(true);
        if (indexValue == 0)
        {
            costValue = "0.2";
        }
        else if (indexValue == 1)
        {
            costValue = "1";
        }
        else if (indexValue == 2)
        {
            costValue = "1.5";
        }

        var userBalance = await ThirdwebManager.Instance.SDK.Wallet.GetBalance();
        if (ConvertStringToRoundedInt(userBalance.displayValue) < 1)
        {
            buyingStatusText.text = "Not Enough XFI";
        }
        else
        {
            try
            {
                // Thực hiện chuyển tiền, nếu thành công thì tiếp tục xử lý giao diện
                await ThirdwebManager.Instance.SDK.Wallet.Transfer(receiverAddress, costValue);

                // Chỉ thực hiện các thay đổi giao diện nếu chuyển tiền thành công

                freeNFTButton.interactable = true;
                coin500Button.interactable = true;
                coin5000Button.interactable = true;
                coin10000Button.interactable = true;
                backButton.interactable = true;


                if (indexValue == 0)
                {
                    buyingStatusText.text = "+500 Coins";
                    coin500Button.gameObject.SetActive(false);
                    ResourceBoost.Instance.coins += 500;
                }
                else if (indexValue == 1)
                {
                    buyingStatusText.text = "+5,000 Coins";
                    coin5000Button.gameObject.SetActive(false);
                    ResourceBoost.Instance.coins += 5000;
                }
                else if (indexValue == 2)
                {
                    buyingStatusText.text = "+10,000 Coins";
                    coin10000Button.gameObject.SetActive(false);
                    ResourceBoost.Instance.coins += 10000;
                }

                coinBoughtText.text = "Coin Bought: "+ ResourceBoost.Instance.coins.ToString();

            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu có lỗi xảy ra
                Debug.LogError($"Lỗi khi thực hiện chuyển tiền: {ex.Message}");
            }
        }
    }

    public async void ClaimFreeNFT()
    {
        freeNFTButton.interactable = false;
        coin500Button.interactable = false;
        coin5000Button.interactable = false;
        coin10000Button.interactable = false;
        backButton.interactable = false;

        buyingStatusText.text = "Claiming...";
        buyingStatusText.gameObject.SetActive(true);

        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(freeNFTAddressSmartContract);
        List<NFT> nftList = await contract.ERC721.GetOwned(Address);
        if (nftList.Count == 0)
        {
            try
            {
                var result = await contract.ERC721.ClaimTo(Address, 1);
                buyingStatusText.text = "NFT Claimed";
                freeNFTButton.gameObject.SetActive(false);
                coin500Button.interactable = true;
                coin5000Button.interactable = true;
                coin10000Button.interactable = true;
                backButton.interactable = true;
            }
            catch (Exception ex)
            {
                buyingStatusText.text = $"Failed to claim NFT: {ex.Message}";
                Debug.LogError("Error while claiming NFT: " + ex);
                freeNFTButton.interactable = true;
                coin500Button.interactable = true;
                coin5000Button.interactable = true;
                coin10000Button.interactable = true;
                backButton.interactable = true;
            }
        }
        else
        {
            buyingStatusText.text = "You already own this NFT.";
            freeNFTButton.gameObject.SetActive(false);
            coin500Button.interactable = true;
            coin5000Button.interactable = true;
            coin10000Button.interactable = true;
        }
    }
}
